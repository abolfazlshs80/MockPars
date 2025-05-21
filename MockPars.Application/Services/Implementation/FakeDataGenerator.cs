using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace MockPars.Application.Services.Implementation
{

    public static class FakeDataGenerator
    {
        private static readonly Faker Faker = new Faker("fa");

        /// <summary>
        /// تولید داده‌های فیک برای یک مدل با استفاده از Reflection
        /// </summary>
        public static T Generate<T>() where T : new()
        {
            var instance = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    var value = GenerateValueForProperty(property.PropertyType);
                    property.SetValue(instance, value);
                }
            }

            return instance;
        }

        /// <summary>
        /// تولید مقدار فیک برای یک Property بر اساس نوع آن
        /// </summary>
        private static object GenerateValueForProperty(Type propertyType)
        {
            if (propertyType == typeof(string))
            {

                return Faker.Lorem.Text(); // تولید متن تصادفی
            }
            else if (propertyType == typeof(int))
            {
                return Faker.Random.Number(1, 1000); // تولید عدد صحیح تصادفی
            }
            else if (propertyType == typeof(decimal))
            {
                return Faker.Random.Decimal(1, 5000); // تولید عدد اعشاری تصادفی
            }
            else if (propertyType == typeof(DateTime))
            {
                return Faker.Date.Past(); // تولید تاریخ گذشته
            }
            else if (propertyType == typeof(bool))
            {
                return Faker.Random.Bool(); // تولید مقدار بولی تصادفی
            }
            else
            {
                // اگر نوع Property پشتیبانی نشود، مقدار پیش‌فرض برگردانده می‌شود
                return GetDefaultValue(propertyType);
            }
        }

        /// <summary>
        /// دریافت مقدار پیش‌فرض برای یک نوع
        /// </summary>
        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
