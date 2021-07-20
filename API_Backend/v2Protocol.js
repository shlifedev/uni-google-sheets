class ReadSpreadSheetResult {
  constructor(
    jsonObject,
    spreadSheetName,
    spreadSheetID,
    sheetIDList,
    tableTypes
  ) {
    this.instruction = instructions.read_spreadsheet;
    this.jsonObject = jsonObject;
    this.spreadSheetName = spreadSheetName;
    this.spreadSheetID = spreadSheetID;
    this.sheetIDList = sheetIDList;
    this.tableTypes = tableTypes;
  }
}

class GetDriveFolderResult {
  constructor() {
    this.fileId = [];
    this.fileName = [];
    this.fileType = [];
    this.url = [];
    instruction = instructions.search_google_drive;
  }
  addFile(id, name, type, url) {
    this.fileId.push(id);
    this.fileName.push(name);
    this.fileType.push(type);
    this.url.push(url);
  }
}

class CreateDefaultSheetResult {
  constructor(fileID, fileName, fileType, url) {
    this.instruction = instructions.create_default_table;
    this.fileID = fileID;
    this.fileName = fileName;
    this.fileType = fileType;
    this.url = url;
  }
}
class CreateExampleResult {
  constructor(createdFolderId) {
    this.instruction = instructions.copy_example;
    this.createdFolderId = createdFolderId;
  }
}

//파일 ID, 이름, 타입, URL
class WriteObjectResult {
  constructor(message, updated) {
    this.instruction = instructions.write_object;
    this.message = message;
    this.isUpdate = updated;
  }
}
