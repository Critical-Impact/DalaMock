namespace DalaMock.Core.Pickers;

using System;

public interface IExcelRowPickerFactory
{
    ExcelRowPicker Create(Type rowType);
}
