﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace Beedev.Xrm.CrmSvcUtil.Extensions.Configuration{
  internal class CaseInsensitiveEnumConfigConverter<T> : ConfigurationConverterBase
  {
    public override object ConvertFrom(
      ITypeDescriptorContext ctx, CultureInfo ci, object data)
    {
      return Enum.Parse(typeof(T), (string)data, true);
    }
  }
}