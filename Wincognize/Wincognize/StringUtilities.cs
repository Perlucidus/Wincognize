using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace Wincognize
{
    public static class StringUtilities
    {
        public static string ToLiteral(string input)
        {
            using (StringWriter writer = new StringWriter())
            using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                return writer.ToString();
            }
        }
    }
}
