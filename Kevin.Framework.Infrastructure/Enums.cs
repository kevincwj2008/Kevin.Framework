using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kevin.Framework.Infrastructure
{
    public enum EnumPhoneType
    {
        /// <summary>
        /// 电话（手机、座机）
        /// </summary>
        Phone,
        /// <summary>
        /// 手机
        /// </summary>
        Cellphone,
        /// <summary>
        /// 座机
        /// </summary>
        Telephone
    }

    public enum EnumHttpMethod
    {
        Get,
        Post,
        Put,
        Delete
    }
}
