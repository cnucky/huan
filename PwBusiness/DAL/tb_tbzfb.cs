using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Maticsoft.DAL
{
	/// <summary>
	/// 数据访问类:tb_tbzfb
	/// </summary>
	public partial class tb_tbzfbDAL

	{
        public tb_tbzfbDAL()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("tbzfbID", "tb_tbzfb"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int tbzfbID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_tbzfb");
			strSql.Append(" where tbzfbID=@tbzfbID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@tbzfbID", MySqlDbType.Int32)
			};
			parameters[0].Value = tbzfbID;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Maticsoft.Model.tb_tbzfb model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_tbzfb(");
			strSql.Append("tbName,tbPwd,zfbEmail,zfbPayPwd,tbStatus,zfbStatus)");
			strSql.Append(" values (");
			strSql.Append("@tbName,@tbPwd,@zfbEmail,@zfbPayPwd,@tbStatus,@zfbStatus)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@tbName", MySqlDbType.VarChar,255),
					new MySqlParameter("@tbPwd", MySqlDbType.VarChar,255),
					new MySqlParameter("@zfbEmail", MySqlDbType.VarChar,255),
					new MySqlParameter("@zfbPayPwd", MySqlDbType.VarChar,255),
					new MySqlParameter("@tbStatus", MySqlDbType.VarChar,255),
					new MySqlParameter("@zfbStatus", MySqlDbType.VarChar,255)};
			parameters[0].Value = model.tbName;
			parameters[1].Value = model.tbPwd;
			parameters[2].Value = model.zfbEmail;
			parameters[3].Value = model.zfbPayPwd;
			parameters[4].Value = model.tbStatus;
			parameters[5].Value = model.zfbStatus;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Maticsoft.Model.tb_tbzfb model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_tbzfb set ");
			strSql.Append("tbName=@tbName,");
			strSql.Append("tbPwd=@tbPwd,");
			strSql.Append("zfbEmail=@zfbEmail,");
			strSql.Append("zfbPayPwd=@zfbPayPwd,");
			strSql.Append("tbStatus=@tbStatus,");
			strSql.Append("zfbStatus=@zfbStatus");
			strSql.Append(" where tbzfbID=@tbzfbID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@tbName", MySqlDbType.VarChar,255),
					new MySqlParameter("@tbPwd", MySqlDbType.VarChar,255),
					new MySqlParameter("@zfbEmail", MySqlDbType.VarChar,255),
					new MySqlParameter("@zfbPayPwd", MySqlDbType.VarChar,255),
					new MySqlParameter("@tbStatus", MySqlDbType.VarChar,255),
					new MySqlParameter("@zfbStatus", MySqlDbType.VarChar,255),
					new MySqlParameter("@tbzfbID", MySqlDbType.Int32,11)};
			parameters[0].Value = model.tbName;
			parameters[1].Value = model.tbPwd;
			parameters[2].Value = model.zfbEmail;
			parameters[3].Value = model.zfbPayPwd;
			parameters[4].Value = model.tbStatus;
			parameters[5].Value = model.zfbStatus;
			parameters[6].Value = model.tbzfbID;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int tbzfbID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_tbzfb ");
			strSql.Append(" where tbzfbID=@tbzfbID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@tbzfbID", MySqlDbType.Int32)
			};
			parameters[0].Value = tbzfbID;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string tbzfbIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_tbzfb ");
			strSql.Append(" where tbzfbID in ("+tbzfbIDlist + ")  ");
			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.tb_tbzfb GetModel(int tbzfbID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select tbzfbID,tbName,tbPwd,zfbEmail,zfbPayPwd,tbStatus,zfbStatus from tb_tbzfb ");
			strSql.Append(" where tbzfbID=@tbzfbID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@tbzfbID", MySqlDbType.Int32)
			};
			parameters[0].Value = tbzfbID;

			Maticsoft.Model.tb_tbzfb model=new Maticsoft.Model.tb_tbzfb();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.tb_tbzfb DataRowToModel(DataRow row)
		{
			Maticsoft.Model.tb_tbzfb model=new Maticsoft.Model.tb_tbzfb();
			if (row != null)
			{
				if(row["tbzfbID"]!=null && row["tbzfbID"].ToString()!="")
				{
					model.tbzfbID=int.Parse(row["tbzfbID"].ToString());
				}
				if(row["tbName"]!=null)
				{
					model.tbName=row["tbName"].ToString();
				}
				if(row["tbPwd"]!=null)
				{
					model.tbPwd=row["tbPwd"].ToString();
				}
				if(row["zfbEmail"]!=null)
				{
					model.zfbEmail=row["zfbEmail"].ToString();
				}
				if(row["zfbPayPwd"]!=null)
				{
					model.zfbPayPwd=row["zfbPayPwd"].ToString();
				}
				if(row["tbStatus"]!=null)
				{
					model.tbStatus=row["tbStatus"].ToString();
				}
				if(row["zfbStatus"]!=null)
				{
					model.zfbStatus=row["zfbStatus"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select tbzfbID,tbName,tbPwd,zfbEmail,zfbPayPwd,tbStatus,zfbStatus ");
			strSql.Append(" FROM tb_tbzfb ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM tb_tbzfb ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.tbzfbID desc");
			}
			strSql.Append(")AS Row, T.*  from tb_tbzfb T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			MySqlParameter[] parameters = {
					new MySqlParameter("@tblName", MySqlDbType.VarChar, 255),
					new MySqlParameter("@fldName", MySqlDbType.VarChar, 255),
					new MySqlParameter("@PageSize", MySqlDbType.Int32),
					new MySqlParameter("@PageIndex", MySqlDbType.Int32),
					new MySqlParameter("@IsReCount", MySqlDbType.Bit),
					new MySqlParameter("@OrderType", MySqlDbType.Bit),
					new MySqlParameter("@strWhere", MySqlDbType.VarChar,1000),
					};
			parameters[0].Value = "tb_tbzfb";
			parameters[1].Value = "tbzfbID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperMySQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

