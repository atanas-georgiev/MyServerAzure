namespace MyServer.Services.Mappings
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using AutoMapper.QueryableExtensions;

    public static class QueryableExtensions
    {
        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return source.ProjectTo(AutoMapperConfig.Configuration, membersToExpand);
        }

        public static IEnumerable<TDestination> To<TDestination>(
            this IEnumerable source,
            params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return source.AsQueryable().ProjectTo(AutoMapperConfig.Configuration, membersToExpand);
        }
    }
}