using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AutoMapper
{
    public static class GlobalMapper
    {
        static InstanceMapper _instance;
        static InstanceMapper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InstanceMapper();
                return _instance;
            }
        }

        public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            return Instance.CreateMap<TSource, TDestination>();
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return Instance.Map<TSource, TDestination>(source);
        }

        public static TDestination Map<TSource, TDestination>(TSource source, TDestination target)
        {
            return Instance.Map(source, target);
        }
    }

    public class InstanceMapper
    {
        IMapper _mapper;
        IMapper Mapper
        {
            get
            {
                if (_mapper == null || ExpressionAdded)
                {
                    MapperConfiguration config = new MapperConfiguration(ConfigurationExpression);
                    _mapper = config.CreateMapper();
                }
                return _mapper;
            }
        }

        MapperConfigurationExpression ConfigurationExpression;

        bool ExpressionAdded;

        public InstanceMapper()
        {
            _mapper = null;
            ConfigurationExpression = new MapperConfigurationExpression();
            ExpressionAdded = false;
        }

        public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        {
            ExpressionAdded = true;
            return ConfigurationExpression.CreateMap<TSource, TDestination>();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination target)
        {
            return Mapper.Map(source, target);
        }
    }

    public static class AutoMapperExtension
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination, TMember>(this IMappingExpression<TSource, TDestination> mappingExpression, Expression<Func<TDestination, TMember>> destinationMember)
        {
            return mappingExpression.ForMember(destinationMember, opt => opt.Ignore());
        }
    }
}