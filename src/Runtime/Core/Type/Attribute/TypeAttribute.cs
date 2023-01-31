namespace GoogleSheet.Type
{
    public class TypeAttribute : System.Attribute
    {
        public System.Type type;
        public string[] sepractors;

        public TypeAttribute(System.Type Type)
        {
            this.type = Type;
            this.sepractors = new string[] { Type.Name };
        }
        public TypeAttribute(System.Type Type, params string[] TypeName)
        {
            this.type = Type;
            this.sepractors = TypeName;
        }
    }
}