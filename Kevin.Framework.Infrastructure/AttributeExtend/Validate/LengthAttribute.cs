namespace Kevin.Framework.Infrastructure.Validate
{
    /// <summary>
    /// 长度验证
    /// </summary>
    public class LengthAttribute : BaseValidateAttribute
    {
        /// <summary>
        /// 最小长度
        /// </summary>
        private int _Min { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        private int _Max { get; set; }

        /// <summary>
        /// 长度验证
        /// </summary>
        /// <param name="Min">最小长度（默认为0）</param>
        /// <param name="Max">最大长度（默认为0）</param>
        public LengthAttribute(int Min = 0, int Max = 0)
        {
            _Min = Min;
            _Max = Max;
        }

        public override bool Validate(object value)
        {
            if (_Min <= 0 && _Max <= 0)
                return true;
            string str = value != null ? value.ToString() : string.Empty;
            return str.Length >= _Min && str.Length <= _Max;
        }

    }
}
