﻿using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using Microsoft.Owin;

namespace Owin.Routing
{
	using AppFunc = Func<IOwinContext, Func<Task>, Task>;
	using HandlerFunc = Func<IOwinContext, Task>;
	
	/// <summary>
	/// Provides fluent API to register http method handlers.
	/// </summary>
	public sealed class RouteBuilder : IRouteHandler
	{
		internal RouteBuilder(IAppBuilder app)
		{
			App = app;
		}

		private IAppBuilder App { get; set; }
		internal Route Route { get; set; }

		private RouteBuilder Register(string method, AppFunc handler)
		{
			if (handler == null) throw new ArgumentNullException("handler");

			App.Use(async (ctx, next) =>
			{
				if (string.Equals(ctx.Request.Method, method, StringComparison.OrdinalIgnoreCase))
				{
					var httpContext = ctx.HttpContext();
					var data = Route.GetRouteData(httpContext);
					if (data != null)
					{
						ctx.Set(Keys.RouteData, data);
						await handler(ctx, next);
					}
					else
					{
						await next();
					}
				}
				else
				{
					await next();
				}
			});

			return this;
		}

		internal RouteBuilder Register(string method, HandlerFunc handler)
		{
			if (handler == null) throw new ArgumentNullException("handler");

			return Register(method, (ctx, _) => handler(ctx));
		}

		/// <summary>
		/// Registers GET handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Get(HandlerFunc handler)
		{
			return Register(HttpMethod.Get, handler);
		}

		/// <summary>
		/// Registers GET handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Get(AppFunc handler)
		{
			return Register(HttpMethod.Get, handler);
		}

		/// <summary>
		/// Registers POST handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Post(HandlerFunc handler)
		{
			return Register(HttpMethod.Post, handler);
		}

		/// <summary>
		/// Registers POST handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Post(AppFunc handler)
		{
			return Register(HttpMethod.Post, handler);
		}

		/// <summary>
		/// Registers PUT handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Put(HandlerFunc handler)
		{
			return Register(HttpMethod.Put, handler);
		}

		/// <summary>
		/// Registers PUT handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Put(AppFunc handler)
		{
			return Register(HttpMethod.Put, handler);
		}

		/// <summary>
		/// Registers UPDATE handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		[Obsolete]
		public RouteBuilder Update(HandlerFunc handler)
		{
			return Register("UPDATE", handler);
		}

		/// <summary>
		/// Registers UPDATE handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		[Obsolete]
		public RouteBuilder Update(AppFunc handler)
		{
			return Register("UPDATE", handler);
		}

		/// <summary>
		/// Registers PATCH handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Patch(HandlerFunc handler)
		{
			return Register(HttpMethod.Patch, handler);
		}

		/// <summary>
		/// Registers PATCH handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Patch(AppFunc handler)
		{
			return Register(HttpMethod.Patch, handler);
		}

		/// <summary>
		/// Registers DELETE handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Delete(HandlerFunc handler)
		{
			return Register(HttpMethod.Delete, handler);
		}

		/// <summary>
		/// Registers DELETE handler.
		/// </summary>
		/// <param name="handler">The handler to register.</param>
		public RouteBuilder Delete(AppFunc handler)
		{
			return Register(HttpMethod.Delete, handler);
		}

		IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
		{
			throw new NotImplementedException();
		}
	}
}
