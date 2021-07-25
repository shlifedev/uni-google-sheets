using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheets.Database.GoogleSheet
{
    public class Issue
    {
        /// <summary>
        /// 있는경우에만 넣기
        /// </summary>
        public System.Exception Exception;
        /// <summary>
        /// 원인을 작성
        /// </summary> 
        public string IssueMessage;
        /// <summary>
        /// 해결방법(가능한경우)
        /// </summary>
        public string ResolveMessage;
        /// <summary>
        /// 해결하기(가능한경우)
        /// </summary>
        public System.Action Resovle;
    }
    public static class IssueCollector
    {
        public static List<Issue> issueList = new List<Issue>();
        public static void AddIssue(string issueMsg, System.Exception exception = null, string resolveMsg = null, System.Action resolve = null) {
            Issue issue = new Issue();
            issue.IssueMessage = issueMsg;
            issue.ResolveMessage = resolveMsg;
            issue.Resovle = resolve;
            issue.Exception = exception;
        }

        public static void TryResolve(Issue issue)
        {
            issue.Resovle?.Invoke();
        }


        public static List<Issue> GetIssues()
        {
            return issueList;
        }

        
    }
}
