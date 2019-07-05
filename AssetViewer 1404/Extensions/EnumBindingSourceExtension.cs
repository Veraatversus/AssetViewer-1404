﻿using System;
using System.Windows.Markup;

namespace AssetViewer1404 {

  public class EnumBindingSourceExtension : MarkupExtension {

    #region Properties

    public Type EnumType {
      get {
        return this._enumType;
      }
      set {
        if (value != this._enumType) {
          if (null != value) {
            Type enumType = Nullable.GetUnderlyingType(value) ?? value;
            if (!enumType.IsEnum)
              throw new ArgumentException("Type must be for an Enum.");
          }

          this._enumType = value;
        }
      }
    }

    #endregion Properties

    #region Constructors

    public EnumBindingSourceExtension() {
    }
    public EnumBindingSourceExtension(Type enumType) {
      this.EnumType = enumType;
    }

    #endregion Constructors

    #region Methods

    public override object ProvideValue(IServiceProvider serviceProvider) {
      if (null == this._enumType)
        throw new InvalidOperationException("The EnumType must be specified.");

      Type actualEnumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
      Array enumValues = Enum.GetValues(actualEnumType);

      if (actualEnumType == this._enumType)
        return enumValues;

      Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
      enumValues.CopyTo(tempArray, 1);
      return tempArray;
    }

    #endregion Methods

    #region Fields

    private Type _enumType;

    #endregion Fields
  }
}