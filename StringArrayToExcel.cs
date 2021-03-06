public static class Conventer {
	public static byte[] getExcelByte(string[, ] data) {
		using(MemoryStream ms = new MemoryStream()) {
			using(SpreadsheetDocument sd = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook)) {
				var workbook = sd.AddWorkbookPart();
				workbook.Workbook = new Workbook();

				var worksheet = workbook.AddNewPart < WorksheetPart > ();
				worksheet.Worksheet = new Worksheet(new SheetData());

				var sheets = sd.WorkbookPart.Workbook.AppendChild(new Sheets());
				Sheet sheet = new Sheet() {
					Id = sd.WorkbookPart.GetIdOfPart(worksheet),
					SheetId = 1,
					Name = "main"
				};
				sheets.Append(sheet);

				SheetData sheetData = worksheet.Worksheet.GetFirstChild < SheetData > ();

				for (int i = 0; i < data.GetLength(0); i++) {
					for (int j = 0; j < data.GetLength(1); j++) {
						Row row;
						row = new Row() {
							RowIndex = (uint)(i + 1)
						};
						sheetData.Append(row);

						Cell newCell = new Cell() {
							CellReference = numToStr(j + 1) + "" + (i + 1)
						};
						row.InsertBefore(newCell, null);

						newCell.CellValue = new CellValue(data[i, j]);
						newCell.DataType = new EnumValue < CellValues > (CellValues.String);
					}
				}

				sd.Close();
				return ms.ToArray();
			}
		}
	}
}
