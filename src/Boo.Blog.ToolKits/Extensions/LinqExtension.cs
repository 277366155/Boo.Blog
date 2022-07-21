using System;
using System.Linq;
using System.Linq.Expressions;

namespace Boo.Blog.ToolKits.Extensions
{
    public static class LinqExtension
    {
        /// <summary>
        /// Linq组建 Orderby动态表达式  排序可传要排序的字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> source, string propertyName, bool asc = true) where TEntity : class
        {
            string command = asc ? "OrderBy" : "OrderByDescending";

            var type = typeof(TEntity);
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string fieldName, object filedValue, FilterOperateType operType = FilterOperateType.Equal)
        {
            //创建表达式中的参数或变量(如m=>m.IsDelete==false中的m)
            var param = Expression.Parameter(typeof(T), "p");
            //创建表达式中左边的属性(如m=>m.IsDelete==false中的m.IsDelete)
            var left = Expression.Property(param, fieldName);
            //创建表达式中右边的常量表达式(如m=>m.IsDelete==false中的false)
            var right = Expression.Constant(filedValue);
            BinaryExpression body;
            switch (operType)
            {
                default:
                case FilterOperateType.Equal:
                    //创建成二元运算符表达式(如m=>m.IsDelete==false中的==)
                    body = Expression.Equal(left, right);
                    break;
                case FilterOperateType.NotEqual:
                    body = Expression.NotEqual(left, right);
                    break;
                case FilterOperateType.GreaterThan:
                    body = Expression.GreaterThan(left, right);
                    break;
                case FilterOperateType.LessThan:
                    body = Expression.LessThan(left, right);
                    break;
            }
            //拼接成筛选条件
            return query.Where(Expression.Lambda<Func<T, bool>>(body, param));
        }
    }
    public enum FilterOperateType : byte
    {
        Equal = 0,
        NotEqual,
        LessThan,
        GreaterThan,
    }
}

