const _callbacks = {};
_callbacks[instructions.write_object] = doWriteObject;

/* UGS Currently no use post method */
function post(e) {  
  const data = JSON.parse(e.postData.contents); 
  const callback = _callbacks[data.instruction]; 
  if (callback === undefined || callback === null)
    return json({ message: "hamster google spread sheet api internal error" });
  return callback(e);
}

/* 새로운 오브젝트를 시트에 쓰거나, 기존 오브젝트를 수정합니다. */
function doWriteObject(e) { 
  const data = JSON.parse(e.postData.contents);
  const {fileID, sheetID, key, value} = data;
  return v2WriteObject(fileID, sheetID, key, value);
}
