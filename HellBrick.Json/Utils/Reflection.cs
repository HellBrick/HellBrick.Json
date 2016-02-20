using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Utils
{
	public static class Reflection
	{
		public static MethodInfo Method( Expression<Action> expr ) => ExtractMethod( expr as LambdaExpression );
		public static MethodInfo Method<TArg>( Expression<Action<TArg>> expr ) => ExtractMethod( expr as LambdaExpression );
		public static MethodInfo Method<TArg1, TArg2>( Expression<Action<TArg1, TArg2>> expr ) => ExtractMethod( expr as LambdaExpression );
		public static MethodInfo Method<TArg1, TArg2, TArg3>( Expression<Action<TArg1, TArg2, TArg3>> expr ) => ExtractMethod( expr as LambdaExpression );
		public static MethodInfo Method<TArg1, TArg2, TArg3, TArg4>( Expression<Action<TArg1, TArg2, TArg3, TArg4>> expr ) => ExtractMethod( expr as LambdaExpression );
		public static MethodInfo Method<TArg>( Expression<Func<TArg>> expr ) => ExtractMethod( expr as LambdaExpression );
		public static MethodInfo Method<TArg1, TArg2>( Expression<Func<TArg1, TArg2>> expr ) => ExtractMethod( expr as LambdaExpression );
		public static MethodInfo Method<TArg1, TArg2, TArg3>( Expression<Func<TArg1, TArg2, TArg3>> expr ) => ExtractMethod( expr as LambdaExpression );
		public static MethodInfo Method<TArg1, TArg2, TArg3, TArg4>( Expression<Func<TArg1, TArg2, TArg3, TArg4>> expr ) => ExtractMethod( expr as LambdaExpression );
		private static MethodInfo ExtractMethod( LambdaExpression expr ) => ( expr.Body as MethodCallExpression ).Method;

		public static PropertyInfo Property<TProperty>( Expression<Func<TProperty>> expr ) => ExtractProperty( expr as LambdaExpression );
		public static PropertyInfo Property<TObject, TProperty>( Expression<Func<TObject, TProperty>> expr ) => ExtractProperty( expr as LambdaExpression );
		private static PropertyInfo ExtractProperty( LambdaExpression lambdaExpression ) => ( lambdaExpression.Body as MemberExpression ).Member as PropertyInfo;
	}
}
