/* drive folder , sheet read, sheet write, create example, create default table */

function v2GetDriveFolder(folderId) {
  result = new GetDriveFolderResult();
  var rootFolder = DriveApp.getFolderById(folderID);
  var folderFiles = rootFolder.getFiles();
  var folderDirectories = rootFolder.getFolders();

  while (folderFiles.hasNext()) {
    var file = folderFiles.next();
    var mimeType = file.getMimeType();
    //스프레드 시트에 대한 처리
    if (mimeType == "application/vnd.google-apps.spreadsheet") {
      var fID = file.getId();
      result.addFile(fID, file.getName(), 2, file.getUrl());
    }
    //폴더에 대한 처리
    if (mimeType == "application/vnd.google-apps.folder") {
      var fID = file.getId();
      result.addFile(fID, file.getName(), 0, file.getUrl());
    }
  }

  while (folderDirectories.hasNext()) {
    var file = folderDirectories.next();
    var fID = file.getId();
    result.addFile(fID, file.getName(), 0, file.getUrl());
  }

  return JSON.stringify(result);
}

function v2ReadSheet(fileID) {
  var spreadSheet = SpreadsheetApp.openById(fileID);
  var sheetIDList = [];
  var sheetTableTypes = [];
  var spreadSheetID = fileID;
  var sheets = spreadSheet.getSheets();
  var jsonObject = {};
  for (var i = 0; i < sheets.length; i++) {
    var sheet = sheets[i];
    var sheetName = sheet.getName();
    var sheetID = sheet.getSheetId();
    var currentTableType = 0;
    if (sheetName.startsWith("#")) continue;
    if (sheetName.startsWith("Enum.")) {
      currentTableType = 1;
    } else {
      currentTableType = 0;
    }
    sheetIDList.push(sheetID);
    sheetTableTypes.push(currentTableType);

    if (sheetName.startsWith("@", 0) == true) continue;
    else {
      var columCount = sheet.getLastColumn();
      var maxRow = sheet.getLastRow();
      var range = sheet.getRange(1, 1, maxRow, columCount).getValues();

      //추후 수식에 대한 처리를 해줄 예정이다.
      //수식 및 데이터 확인
      //var formulas = sheet.getRange(1, 1, maxRow, columCount).getFormulas();

      jsonObject[sheetName] = {};
      for (var row = 0; row < maxRow; row++) {
        for (var col = 0; col < columCount; col++) {
          //#는 무시한다.
          if (range[0][col].startsWith("#")) {
            continue;
          }

          if (row == 0) jsonObject[sheetName][range[0][col]] = [];
          else if (row == 1) continue;
          else {
            jsonObject[sheetName][range[0][col]].push(range[row][col]);
          }
        }
      }
    }
  }

  var response = new ReadSpreadSheetResult(
    jsonObject,
    spreadSheet.getName(),
    spreadSheetID,
    sheetIDList,
    sheetTableTypes
  );
  return JSON.stringify(response);
}

function v2WriteObject(fileID, sheetID, key, value) {
  let result = null;
  if (isWriteable() == false) {
    return JSON.stringify({
      message:
        "if you want write add write property and set true in apps script!",
    });
  }

  let spreadSheet = SpreadsheetApp.openById(fileID);
  let sheetIDList = [];
  let data = {};
  let sheets = spreadSheet.getSheets();
  Logger.log("start" + spreadSheet.getName());
  for (var i = 0; i < sheets.length; i++) {
    Logger.log(sheets[i].getSheetId());
    if (sheets[i].getSheetId() == sheetID) {
      var sheet = sheets[i];
      var columCount = sheet.getLastColumn();
      var maxRow = sheet.getLastRow();
      var range = sheet.getRange(1, 1, maxRow, columCount).getValues();

      // Get Key Field [0,0]
      var keyField = range[0][0];

      // Get Current Key
      var currentKey = keyField.split(":")[0];
      currentKey = currentKey.replace(" ", "");
      for (var row = 2; row < maxRow; row++) {
        if (range[row][0] == key) {
          var targetLength = value.length;
          var index = 0;
          for (var x = 0; x < targetLength; x++) {
            // 0열, x행이 #로 시작하는경우
            if (range[0][x].startsWith("#")) {
              //target length를 올려서 for문을 더 돌게한다.
              targetLength++;
              continue;
            }
            sheet.getRange(row + 1, x + 1).setValue(value[index]);
            index++;
          }
          result = new WriteObjectResult("write 200", true);
          return JSON.stringify(result);
        }
      }
      sheet.appendRow(value);
      result = new WriteObjectResult("write 200", false);
      return JSON.stringify(result);
    }
  }

  result = new WriteObjectResult("write 200", true);
  return JSON.stringify(result);
}

function v2CreateExample() {
  var exampleFolder = DriveApp.getFolderById(
    "189ozeSkUZpseEBWCOvGlN4FKefwz25Q6"
  );
  var targetFolder = DriveApp.createFolder("UnityGoogleSheetExample");
  targetFolder.setName("UGS_Example_" + targetFolder.getId());
  var exampleFiles = exampleFolder.getFiles();

  while (exampleFiles.hasNext()) {
    var exampleFile = exampleFiles.next();
    var copied = exampleFile.makeCopy(targetFolder);
    copied.setName(exampleFile.getName());
  }

  var data = new CreateExampleResult(targetFolder.getId());
  return JSON.stringify(data);
}

function v2CreateDefault(folderID, fileName) {
  if (folderID == undefined || folderID == null)
    throw new Error("Folder ID is null!");
  if (fileName == null) fileName = "DefaultTable";
  var sheet = SpreadsheetApp.create(fileName);
  var sheetFileId = sheet.getId();
  var targetFolder = DriveApp.getFolderById(folderID);
  var file = DriveApp.getFileById(sheetFileId);

  var folderFiles = targetFolder.getFiles();

  while (folderFiles.hasNext()) {
    var folderFile = folderFiles.next();
    if (folderFile.getName() == fileName) {
      Logger.log("failed!");
      return "failed";
    }
  }

  file.setName(fileName);
  file.moveTo(targetFolder);
  sheet
    .getSheets()[0]
    .appendRow(["index : int", "intValue : int", "strValue : string"]);
  var a1c1 = sheet.getSheets()[0].getRange("A1:C1");
  a1c1.setBackground("black");
  a1c1.setFontColor("white");

  sheet.getSheets()[0].appendRow(["description", "desc", "desc"]);
  var a2c2 = sheet.getSheets()[0].getRange("A2:C2");
  a2c2.setBackground("gray");
  a2c2.setFontColor("white");

  sheet.setName(fileName);
  sheet.getSheets()[0].appendRow(["0", "100", "String"]);
  sheet.getSheets()[0].appendRow(["1", "50", "String2"]);
  sheet.getSheets()[0].appendRow(["2", "200", "Hello"]);
  sheet.getSheets()[0].appendRow(["3", "150", "World"]);

  var range = sheet.getSheets()[0].getRange("A1:Z1000");
  range.setHorizontalAlignment("center");

  sheet.getSheets()[0].setName("Data");
  const result = new CreateDefaultSheetResult(
    file.getId(),
    file.getName(),
    2,
    file.getUrl()
  );
  return JSON.stringify(result);
}
