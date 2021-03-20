namespace Hamster.ZG.Type
{
    public class TypeAttribute : System.Attribute
    {
        public System.Type type;
        public string[] sepractors;  
        public TypeAttribute(System.Type type, params string[] speractors)
        {
            this.type = type;
            this.sepractors = speractors;
        }
    }
}