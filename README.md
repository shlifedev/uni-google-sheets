# UnityGoogleSheet
 
GoogleScript + GoogleSprheadSheet 를 활용한 뛰어난 게임 테이블 매니저

## 기능
- 리플렉션을 적극 활용하여 프로그래머가 매우 쉽게 타입을 추가할 수 있음

 벡터3 타입 추가 예제  
 일반적으로 테이블에서 관리하기 힘든 타입이지만 매우 쉽게 추가할 수 있습니다.
 ```csharp
    [Type(typeof(Vector3), new string[] { "Vector3", "vector3"})]
    public struct Vector3Type : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
             // write your read code :D
        }
    }
 ```
 튜플 타입 추가 예제  
 일반적으로 테이블에서 관리하기 힘든 타입이지만 매우 쉽게 추가할 수 있습니다.
 ```csharp
    [Type(typeof((int, int)), new string[] { "(int,int)", "(Int32,Int32)" })]
    public class IntTupleX2Type : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            var datas = ReadUtil.GetBracketValueToArray(value);
            if(datas.Length == 0 || datas.Length == 1 || datas.Length > 2) return DefaultValue;
            else 
                return (datas[0], datas[1]); 
        }
    }
 ```
 
 - 빠른 성능 및 안정성
 - 런타임 및 에디터 타임에서 데이터를 불러오고 수정할 수 있음
 - 런타임 및 에디터 에서 수정한 데이터를 구글시트에 실시간 저장할 수 있음
 - 매우 직관적인 에디터 GUI지원
 
