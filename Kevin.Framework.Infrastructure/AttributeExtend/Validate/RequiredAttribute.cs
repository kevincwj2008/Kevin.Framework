namespace Kevin.Framework.Infrastructure.Validate
{
    /// <summary>
    /// 必填验证
    /// </summary>
    public class RequiredAttribute : BaseValidateAttribute
    {
        public override bool Validate(object value)
        {
            return value != null && string.IsNullOrWhiteSpace(value.ToString());
        }
    }
}
