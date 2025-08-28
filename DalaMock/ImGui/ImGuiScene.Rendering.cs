namespace DalaMock.Core.Imgui;

using System;
using System.Numerics;
using System.Runtime.CompilerServices;

using Dalamud.Bindings.ImGui;

using Veldrid;

/// <summary>
/// This file contains everything related to rendering to the window.
/// </summary>
public partial class ImGuiScene
{
    private readonly ImTextureID fontAtlasId = 1;
    private Texture? fontTexture;
    private ResourceSet? fontTextureResourceSet;
    private TextureView fontTextureView;
    private Shader fragmentShader;
    private DeviceBuffer indexBuffer;
    private ResourceLayout layout;
    private ResourceSet mainResourceSet;
    private Pipeline pipeline;
    private DeviceBuffer projMatrixBuffer;
    private ResourceLayout textureLayout;
    private DeviceBuffer vertexBuffer;
    private Shader vertexShader;

    /// <summary>
    /// Creates the graphics rendering pipeline.
    /// </summary>
    /// <param name="gd">The veldrid graphics device.</param>
    /// <param name="outputDescription">The frame buffer's output description.</param>
    public void CreateDeviceResources(GraphicsDevice gd, OutputDescription outputDescription)
    {
        var factory = gd.ResourceFactory;
        this.vertexBuffer =
            factory.CreateBuffer(new BufferDescription(10000, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
        this.vertexBuffer.Name = "ImGui.NET Vertex Buffer";
        this.indexBuffer =
            factory.CreateBuffer(new BufferDescription(2000, BufferUsage.IndexBuffer | BufferUsage.Dynamic));
        this.indexBuffer.Name = "ImGui.NET Index Buffer";

        this.projMatrixBuffer =
            factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));
        this.projMatrixBuffer.Name = "ImGui.NET Projection Buffer";

        var vertexShaderBytes = this.LoadEmbeddedShaderCode(gd.ResourceFactory, "imgui-vertex", ShaderStages.Vertex);
        var fragmentShaderBytes = this.LoadEmbeddedShaderCode(gd.ResourceFactory, "imgui-frag", ShaderStages.Fragment);
        this.vertexShader = factory.CreateShader(
            new ShaderDescription(
                ShaderStages.Vertex,
                vertexShaderBytes,
                gd.BackendType == GraphicsBackend.Metal ? "VS" : "main"));
        this.fragmentShader = factory.CreateShader(
            new ShaderDescription(
                ShaderStages.Fragment,
                fragmentShaderBytes,
                gd.BackendType == GraphicsBackend.Metal ? "FS" : "main"));

        VertexLayoutDescription[] vertexLayouts =
        {
            new(
                new VertexElementDescription("in_position", VertexElementSemantic.Position, VertexElementFormat.Float2),
                new VertexElementDescription(
                    "in_texCoord",
                    VertexElementSemantic.TextureCoordinate,
                    VertexElementFormat.Float2),
                new VertexElementDescription("in_color", VertexElementSemantic.Color, VertexElementFormat.Byte4_Norm)),
        };

        this.layout = factory.CreateResourceLayout(
            new ResourceLayoutDescription(
                new ResourceLayoutElementDescription(
                    "ProjectionMatrixBuffer",
                    ResourceKind.UniformBuffer,
                    ShaderStages.Vertex),
                new ResourceLayoutElementDescription("MainSampler", ResourceKind.Sampler, ShaderStages.Fragment)));
        this.textureLayout = factory.CreateResourceLayout(
            new ResourceLayoutDescription(
                new ResourceLayoutElementDescription(
                    "MainTexture",
                    ResourceKind.TextureReadOnly,
                    ShaderStages.Fragment)));
        this.RecreateFontDeviceTexture(gd);

        var pd = new GraphicsPipelineDescription(
            BlendStateDescription.SingleAlphaBlend,
            new DepthStencilStateDescription(false, false, ComparisonKind.Always),
            new RasterizerStateDescription(FaceCullMode.None, PolygonFillMode.Solid, FrontFace.Clockwise, false, true),
            PrimitiveTopology.TriangleList,
            new ShaderSetDescription(vertexLayouts, new[] { this.vertexShader, this.fragmentShader }),
            new[] { this.layout, this.textureLayout },
            outputDescription,
            ResourceBindingModel.Default);
        this.pipeline = factory.CreateGraphicsPipeline(ref pd);

        this.mainResourceSet = factory.CreateResourceSet(
            new ResourceSetDescription(
                this.layout,
                this.projMatrixBuffer,
                gd.LinearSampler));
    }

    /// <summary>
    /// Recreates the font texture, used when adding a new font.
    /// </summary>
    /// <param name="gd">Veldrid's graphics device.</param>
    public unsafe void RecreateFontDeviceTexture(GraphicsDevice gd)
    {
        var io = ImGui.GetIO();

        // Build
        byte* pixels = null;
        int width = 0, height = 0, bytesPerPixel = 0;
        io.Fonts.GetTexDataAsRGBA32(0, ref pixels, ref width, ref height, ref bytesPerPixel);

        // Store our identifier
        io.Fonts.SetTexID(0, this.fontAtlasId);

        this.fontTexture?.Dispose();
        this.fontTexture = gd.ResourceFactory.CreateTexture(
            TextureDescription.Texture2D(
                (uint)width,
                (uint)height,
                1,
                1,
                PixelFormat.R8_G8_B8_A8_UNorm,
                TextureUsage.Sampled));
        this.fontTexture.Name = "ImGui.NET Font Texture";
        gd.UpdateTexture(
            this.fontTexture,
            (IntPtr)pixels,
            (uint)(bytesPerPixel * width * height),
            0,
            0,
            0,
            (uint)width,
            (uint)height,
            1,
            0,
            0);
        this.fontTextureResourceSet?.Dispose();
        this.fontTextureView = gd.ResourceFactory.CreateTextureView(this.fontTexture);
        this.fontTextureResourceSet = gd.ResourceFactory.CreateResourceSet(new ResourceSetDescription(this.textureLayout, this.fontTextureView));
        this.fontTextureResourceSet.Name = "ImGui.NET Font Texture Resource Set";

        io.Fonts.ClearTexData();
    }

    /// <summary>
    /// Retrieves the shader texture binding for the given helper handle.
    /// </summary>
    /// <param name="imGuiBinding">A pointer to the imgui binding.</param>
    /// <returns>Returns a image resource set.</returns>
    public ResourceSet GetImageResourceSet(IntPtr imGuiBinding)
    {
        if (!this.viewsById.TryGetValue(imGuiBinding, out var tvi))
        {
            throw new InvalidOperationException("No registered ImGui binding with id " + imGuiBinding);
        }

        return tvi.ResourceSet;
    }

    private unsafe void RenderImDrawData(ImDrawDataPtr drawData, GraphicsDevice gd, CommandList cl)
    {
        uint vertexOffsetInVertices = 0;
        uint indexOffsetInElements = 0;

        if (drawData.CmdListsCount == 0)
        {
            return;
        }

        var totalVbSize = (uint)(drawData.TotalVtxCount * Unsafe.SizeOf<ImDrawVert>());
        if (totalVbSize > this.vertexBuffer.SizeInBytes)
        {
            gd.DisposeWhenIdle(this.vertexBuffer);
            this.vertexBuffer = gd.ResourceFactory.CreateBuffer(
                new BufferDescription(
                    (uint)(totalVbSize * 1.5f),
                    BufferUsage.VertexBuffer | BufferUsage.Dynamic));
        }

        var totalIbSize = (uint)(drawData.TotalIdxCount * sizeof(ushort));
        if (totalIbSize > this.indexBuffer.SizeInBytes)
        {
            gd.DisposeWhenIdle(this.indexBuffer);
            this.indexBuffer = gd.ResourceFactory.CreateBuffer(
                new BufferDescription(
                    (uint)(totalIbSize * 1.5f),
                    BufferUsage.IndexBuffer | BufferUsage.Dynamic));
        }

        for (var i = 0; i < drawData.CmdListsCount; i++)
        {
            var cmdList = drawData.CmdLists[i];

            cl.UpdateBuffer(
                this.vertexBuffer,
                vertexOffsetInVertices * (uint)Unsafe.SizeOf<ImDrawVert>(),
                (IntPtr)cmdList->VtxBuffer.Data,
                (uint)(cmdList->VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>()));

            cl.UpdateBuffer(
                this.indexBuffer,
                indexOffsetInElements * sizeof(ushort),
                (IntPtr)cmdList->IdxBuffer.Data,
                (uint)(cmdList->IdxBuffer.Size * sizeof(ushort)));

            vertexOffsetInVertices += (uint)cmdList->VtxBuffer.Size;
            indexOffsetInElements += (uint)cmdList->IdxBuffer.Size;
        }

        // Setup orthographic projection matrix into our constant buffer
        var io = ImGui.GetIO();
        var mvp = Matrix4x4.CreateOrthographicOffCenter(
            0f,
            io.DisplaySize.X,
            io.DisplaySize.Y,
            0.0f,
            -1.0f,
            1.0f);

        this.GraphicsDevice.UpdateBuffer(this.projMatrixBuffer, 0, ref mvp);

        cl.SetVertexBuffer(0, this.vertexBuffer);
        cl.SetIndexBuffer(this.indexBuffer, IndexFormat.UInt16);
        cl.SetPipeline(this.pipeline);
        cl.SetGraphicsResourceSet(0, this.mainResourceSet);

        drawData.ScaleClipRects(io.DisplayFramebufferScale);

        // Render command lists
        var vtxOffset = 0;
        var idxOffset = 0;
        for (var n = 0; n < drawData.CmdListsCount; n++)
        {
            var cmdList = drawData.CmdLists[n];
            for (var cmdI = 0; cmdI < cmdList->CmdBuffer.Size; cmdI++)
            {
                var pcmd = cmdList->CmdBuffer[cmdI];
                if (pcmd.UserCallback != (void*)IntPtr.Zero)
                {
                    throw new NotImplementedException();
                }

                if (pcmd.TextureId != IntPtr.Zero)
                {
                    if (pcmd.TextureId == this.fontAtlasId)
                    {
                        cl.SetGraphicsResourceSet(1, this.fontTextureResourceSet);
                    }
                    else
                    {
                        cl.SetGraphicsResourceSet(1, this.GetImageResourceSet((IntPtr)pcmd.TextureId.Handle));
                    }
                }

                cl.SetScissorRect(
                    0,
                    (uint)pcmd.ClipRect.X,
                    (uint)pcmd.ClipRect.Y,
                    (uint)(pcmd.ClipRect.Z - pcmd.ClipRect.X),
                    (uint)(pcmd.ClipRect.W - pcmd.ClipRect.Y));

                cl.DrawIndexed(
                    pcmd.ElemCount,
                    1,
                    pcmd.IdxOffset + (uint)idxOffset,
                    (int)pcmd.VtxOffset + vtxOffset,
                    0);
            }

            vtxOffset += cmdList->VtxBuffer.Size;
            idxOffset += cmdList->IdxBuffer.Size;
        }
    }

    private byte[] LoadEmbeddedShaderCode(ResourceFactory factory, string name, ShaderStages stage)
    {
        switch (factory.BackendType)
        {
            case GraphicsBackend.Direct3D11:
                {
                    var resourceName = name + ".hlsl.bytes";
                    return this.GetEmbeddedResourceBytes(resourceName);
                }

            case GraphicsBackend.OpenGL:
                {
                    var resourceName = name + ".glsl";
                    return this.GetEmbeddedResourceBytes(resourceName);
                }

            case GraphicsBackend.Vulkan:
                {
                    var resourceName = name + ".spv";
                    return this.GetEmbeddedResourceBytes(resourceName);
                }

            case GraphicsBackend.Metal:
                {
                    var resourceName = name + ".metallib";
                    return this.GetEmbeddedResourceBytes(resourceName);
                }

            default:
                throw new NotImplementedException();
        }
    }

    private byte[] GetEmbeddedResourceBytes(string resourceName)
    {
        var assembly = typeof(ImGuiScene).Assembly;
        using (var s = assembly.GetManifestResourceStream(resourceName))
        {
            if (s == null)
            {
                throw new Exception($"Failed to get embedded resource {resourceName}.");
            }

            var ret = new byte[s.Length];
            s.ReadExactly(ret, 0, (int)s.Length);
            return ret;
        }
    }
}
