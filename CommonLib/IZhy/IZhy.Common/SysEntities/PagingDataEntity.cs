﻿using IZhy.Common.BasicTools;
using System.Collections;
using System.Data;

namespace IZhy.Common.SysEntities
{
    /// <summary>
    /// <para>分页数据 实体类</para>
    /// <para>必须设置基础数据：</para>
    /// <para>AllDataCount，数据总条数</para>
    /// <para>PageSize，每页数据量</para>
    /// <para>PageNum，当前页码号</para>
    /// </summary>
    public class PagingDataEntity
    {
        /// <summary>
        /// <para>分页实体类初始化基础数据</para>
        /// <para>参数：</para>
        /// <para>allDataCount 数据总条数；pageSize 每页数据量；pageNum 当前页码号</para>
        /// </summary>
        /// <param name="allDataCount">数据总条数</param>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="pageNum">当前页码号</param>
        public PagingDataEntity(int allDataCount, int pageSize, int pageNum)
        {
            AllDataCount = allDataCount;
            PageSize = pageSize;
            PageNum = pageNum;
        }


        /// <summary>
        /// 每页的数据量，默认数量值 10
        /// </summary>
        public const int DefaultPageSize = 10;


        /// <summary>
        /// 数据总页数
        /// </summary>
        private int _allPageCount = 0;

        /// <summary>
        /// 当前页码号
        /// </summary>
        private int _pageNum = 1;

        /// <summary>
        /// 每页数据量
        /// </summary>
        private int _pageSize = DefaultPageSize;

        /// <summary>
        /// 发起分页查询的开始序号
        /// </summary>
        private int _startNum = 0;


        /// <summary>
        /// 数据总条数
        /// </summary>
        public int AllDataCount { get; set; } = 0;

        /// <summary>
        /// 数据总页数
        /// </summary>
        public int AllPageCount
        {
            get
            {
                _allPageCount = AllDataCount % PageSize == 0 ? AllDataCount / PageSize : (AllDataCount / PageSize) + 1;
                return _allPageCount;
            }
        }

        /// <summary>
        /// 当前页码号
        /// </summary>
        public int PageNum
        {
            get
            {
                if (_pageNum < 1)
                {
                    _pageNum = 1;
                }
                else if (_pageNum > AllPageCount)
                {
                    _pageNum = AllPageCount;
                }
                return _pageNum;
            }
            set { _pageNum = value; }
        }

        /// <summary>
        /// 每页数据量；1 ~ 1000
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize < 1)
                {
                    _pageSize = DefaultPageSize;
                }

                if (_pageSize > 1000)
                {
                    _pageSize = 1000;
                }

                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        /// <summary>
        /// 发起分页查询的开始序号
        /// </summary>
        public int StartNum
        {
            get
            {
                if (PageNum < 2)
                {
                    _startNum = 0;
                }
                else
                {
                    _startNum = (PageNum - 1) * PageSize;
                }
                return _startNum;
            }
        }

        /// <summary>
        /// 结果数据的数据量
        /// <para>结果值：= -1 则表示结果数据可能不是有效的集合数据，需要检查程序代码</para>
        /// </summary>
        public int ResultDataCount
        {
            get
            {
                if (ResultData == null)
                {
                    return 0;
                }
                else
                {
                    if (ResultData is DataTable)
                    {
                        return (ResultData as DataTable).Rows.Count;
                    }
                    else if (ResultData is ICollection)
                    {
                        return (ResultData as ICollection).Count;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        /// <summary>
        /// 分页查询的结果数据
        /// </summary>
        public object ResultData { get; set; } = null;


        public override string ToString()
        {
            return JsonTool.ObjectToJson(this);
        }
    }
}
