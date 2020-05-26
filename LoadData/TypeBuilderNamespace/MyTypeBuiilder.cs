using LoadData.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;

namespace LoadData.TypeBuilderNamespace
{
   
        public static class MyTypeBuilder
        {
            public static object CreateNewObject(List<IDynamicClassFields> yourListOfFields, string className)
            {
                var myType = CompileResultType(yourListOfFields, className);
            object[] attributes = myType.Assembly.GetCustomAttributes(true);
            Console.WriteLine("MyAttribute custom attribute contains : ");
            for (int index = 0; index < attributes.Length; index++)
            {
                if (attributes[index] is ModelClassAttribute)
                {
                    Console.WriteLine("s : " + ((ModelClassAttribute)attributes[index]));
                    Console.WriteLine("x : " + ((ModelClassAttribute)attributes[index]));
                    break;
                }
            }


            var myObject = Activator.CreateInstance(myType);

            return myObject;
            }
            public static Type CompileResultType(List<IDynamicClassFields> yourListOfFields, string className)
            {
                TypeBuilder tb = GetTypeBuilder(className);
                ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

                // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
                foreach (var field in yourListOfFields)
                    CreateProperty(tb, field.FieldName, field.FieldType);

                Type objectType = tb.CreateType();
                return objectType;
            }

            private static TypeBuilder GetTypeBuilder( string className)
            {
                var typeSignature = className;
                var an = new AssemblyName(typeSignature);
                AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            Type myType = typeof(ModelClassAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] {  });
            CustomAttributeBuilder attributeBuilder =
         new CustomAttributeBuilder(infoConstructor,new object[] { });
            //assemblyBuilder.SetCustomAttribute(attributeBuilder);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
                TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                        TypeAttributes.Public |
                        TypeAttributes.Class |
                        TypeAttributes.AutoClass |
                        TypeAttributes.AnsiClass |
                        TypeAttributes.BeforeFieldInit |
                        TypeAttributes.AutoLayout ,
                        null);
                 tb.SetCustomAttribute(attributeBuilder);
                return tb;
            }

            private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
            {
                FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

                PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);


            Type[] keyAttributeAttributeParams = new Type[] { };
            ConstructorInfo keyAttrInfo = typeof(KeyAttribute).GetConstructor(keyAttributeAttributeParams);
            CustomAttributeBuilder KeyAttributeBuilder = new CustomAttributeBuilder(keyAttrInfo, new object[] {  });
            propertyBuilder.SetCustomAttribute(KeyAttributeBuilder);


            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
                ILGenerator getIl = getPropMthdBldr.GetILGenerator();

                getIl.Emit(OpCodes.Ldarg_0);
                getIl.Emit(OpCodes.Ldfld, fieldBuilder);
                getIl.Emit(OpCodes.Ret);

                MethodBuilder setPropMthdBldr =
                    tb.DefineMethod("set_" + propertyName,
                      MethodAttributes.Public |
                      MethodAttributes.SpecialName |
                      MethodAttributes.HideBySig,
                      null, new[] { propertyType });

                ILGenerator setIl = setPropMthdBldr.GetILGenerator();
                Label modifyProperty = setIl.DefineLabel();
                Label exitSet = setIl.DefineLabel();

                setIl.MarkLabel(modifyProperty);
                setIl.Emit(OpCodes.Ldarg_0);
                setIl.Emit(OpCodes.Ldarg_1);
                setIl.Emit(OpCodes.Stfld, fieldBuilder);

                setIl.Emit(OpCodes.Nop);
                setIl.MarkLabel(exitSet);
                setIl.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getPropMthdBldr);
                propertyBuilder.SetSetMethod(setPropMthdBldr);
            }
       



    }
}
