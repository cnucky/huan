using System;
namespace JiShi.Model
{
	/// <summary>
	/// js_convert:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class js_convert
	{
		public js_convert()
		{}
		#region Model
		private int _js_id;
		private string _js_teype;
		private string _js_source;
		private string _js_dest;
		/// <summary>
		/// 
		/// </summary>
		public int js_id
		{
			set{ _js_id=value;}
			get{return _js_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string js_teype
		{
			set{ _js_teype=value;}
			get{return _js_teype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string js_source
		{
			set{ _js_source=value;}
			get{return _js_source;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string js_dest
		{
			set{ _js_dest=value;}
			get{return _js_dest;}
		}
		#endregion Model

	}
}

