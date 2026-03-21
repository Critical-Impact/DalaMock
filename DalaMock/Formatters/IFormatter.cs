namespace DalaMock.Core.Formatters;

using Lumina.Excel;

public interface IExcelRowFormatter<in T>
    where T : struct, IExcelRow<T>
{
    string Format(IExcelRow<T> row);
}
