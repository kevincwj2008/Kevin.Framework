namespace Kevin.Framework.Infrastructure
{
    /// <summary>
    /// Newtonsoft.Json扩展（返回key转小写）
    /// </summary>
    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}