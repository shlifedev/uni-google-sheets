using System; 
using GoogleSheet.Reflection;
using GoogleSheet.Protocol.v2.Req;

namespace GoogleSheet.Validator
{
    public class CompareSheetId : UGSValidator
    {
        public override void isValid(Action<bool> callback, params object[] objs)
        {
            try
            {
                if (objs == null || objs.Length == 0) throw new UGSValidateException("validate params is empty");
                var driveID = objs[0] as string;
                var reqMdl = new GetDriveDirectoryReqModel(driveID);
                requester.GetDriveDirectory(reqMdl, err =>
                { 
                    throw err;
                }, driveDirectory =>
                {
                    int idx = 0;
                    driveDirectory.fileId.ForEach(driveSpreadID => { 
                        var driveSpreadName = driveDirectory.fileName[idx];
                        var driveFileType = driveDirectory.fileType[idx];
                        if (driveFileType == 2)
                        {
                            var tables = Utility.GetAllSubclassOf(typeof(ITable));
                            bool isValid = false;
                            foreach (var localTable in tables)
                            {
                                var localSpreadID = TableUtils.GetSpreadSheetID(localTable);
                                if (driveSpreadID == localSpreadID) 
                                    isValid = true; 
                            } 
                            if(isValid == false)
                            {
                                throw new UGSValidateException("Drive Spread Sheet ID != Local Spread Sheet ID "); 
                            }
                        } 
                        idx++;
                    });

                    callback?.Invoke(true);
                });
            }
            catch (System.Exception e)
            {
                callback?.Invoke(false);
            }

        }
    }
}
