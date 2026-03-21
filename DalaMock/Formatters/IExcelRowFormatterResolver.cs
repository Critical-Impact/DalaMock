namespace DalaMock.Core.Formatters;

using Lumina.Excel;

public interface IExcelRowFormatterResolver
{
    string Format<T>(T row)
        where T : struct, IExcelRow<T>;
}
