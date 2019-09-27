using System;
using System.Collections.Generic;
using System.Text;

namespace ActioBP.General.Objects
{
    public class PropertyCopier<TParent, TChild> where TParent : class where TChild : class {
        public static void Copy(TParent parent, TChild child)
        {
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = child.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (String.Compare(parentProperty.Name, childProperty.Name, StringComparison.OrdinalIgnoreCase) == 0 && parentProperty.PropertyType == childProperty.PropertyType)
                    {
                        if (childProperty.CanWrite && parentProperty.CanRead)
                        {
                            childProperty.SetValue(child, parentProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
        }
    }

}
