const callbacks = {};

/* initialize callbacks */
callbacks[instructions.search_google_drive] = doGetGoogleDrive;
callbacks[instructions.read_spreadsheet] = doGetSpreadSheet;
callbacks[instructions.write_object] = doWriteObject;
callbacks[instructions.create_default_table] = doCreateDefault;
callbacks[instructions.copy_example] = doCreateExample;

function get(e) {  
  const callback = callbacks[e.parameter.instruction];
  if (callback === undefined || callback === null)
    return json({message:"hamster google spread sheet api internal error"});
  return callback(e);
}

/* 예제1~4번 테이블 생성 */
function doCreateExample(e) {
  return copyExampleSheets();
}

/* 기본 테이블 생성 */
function doCreateDefault(e) {
  const { folderID, fileName } = e.parameter;
  return copyExampleSheets(folderID, fileName);
}

/* 새로운 오브젝트를 시트에 쓰거나, 기존 오브젝트를 수정합니다. */
function doWriteObject(e) {
  const { fileID, sheetID, key, value } = e.parameter;
  return writeData(fileID, sheetID, key, value);
}

/* 구글 드라이브 폴더 데이터를 가져옵니다. */
function doGetGoogleDrive(e) {
  return "zz";
  const { folderId } = e.parameter;
  const driveDirectoryInfo = getFolderInfo(folderId);
  return json(driveDirectoryInfo);
}

/* 스프레드시트 가져오기 */
function doGetSpreadSheet(e) {
  const { fileId } = e.parameter;
  const readedSpreadSheetData = sheetToJson(fileId);
  return json(readedSpreadSheetData);
}
