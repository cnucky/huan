using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwBusiness
{


    public enum BusinessStatus {
        ready,
        tb_login_start,
        tb_login_ing,
        tb_login_end,
        tb_buypage,
        tb_buy_confirm,
        tb_buy_zhifu,
        tb_buy_zhifu_ok,//支付完成的页面
        /// <summary>
        /// 任意页面，跳转至已购买的列表页面，
        /// </summary>
        tb_nav2bought_list,//
        /// <summary>
        ///已购买的列表页面，跳转至收货/好评/退款/删除等页面
        /// </summary>
        tb_bought_list,
        /// <summary>
        /// 刚进入收货页面时候的页面
        /// </summary>
        tb_nav2confirmUrl1,
        /// <summary>
        /// 
        /// </summary>
        tb_confirm_goods,//收货的页面
        tb_confirm_goods_redrect2_confirm,//收货的页面
        /// <summary>
        /// 当前任意页，跳转到好评页
        /// </summary>
        tb_rate_nav2goods,//当前任意页，跳转到好评页
        tb_remark_seller,//当前好评页，跳转到好评结束页
        /// <summary>
        /// 当前任意结束一次任务页面
        /// </summary>
        tb_finish,

        //163


        wy_163_begin,//nav 2 reg page
        wy_163_vcode,
        wy_163_main,
        zfb_reg_begin,
        zfb_reg_link_complete,//当前任意页，通过邮件中的链接跳过去
        /// <summary>
        /// 当前注册成功页面（填写银行信息），跳转到开通淘宝页面
        /// </summary>
        zfb_reg_sucess,//zfb_Reg_paymethod
        /// <summary>
        /// 当前登录成功的支付宝页 登录淘宝页
        /// </summary>
        zfb_reg_taobao_open,
        zfb_reg_taobao_new_alipay_q,

        new_register,
        new_cellphone_reg_two,
        new_email_reg_two,
        new_email_reg_three,
        regitster_confirm,
        account_management,
        paymethod,
        zfb_Reg_paymethod,
        zfb_reg_skip_bindassetcard,
        zfb_reg_taobao_login2reg,

        zfb_reg_account_reg_success,
        pp_ads_enter,
        pp_ads_enterOk,
        //==============================添加密保
        zfb_reg_nav2set_SecurityQuestion,
        zfb_reg_add_SecurityQuestion_Fill_PayPwd,
        zfb_reg_setQa,
        zfb_reg_add_SecurityQuestion_setQa_confirm,
        zfb_reg_nav2set_add_SecurityQuestion,
        zfb_reg_nav2set_add_SecurityQuestion_before,
        deliver_address,
        nav2deliver_address,
        //shop v2
        mtb,
        gerenzhongxin,
        Awp_core_detail,
        awp_base_buy,
        exCashier,
        asyn_payment_result,
        TB_shop_V2_Ready,
        wapshop_ex_loginerror,
        act_sale_searchlist,
        search_htm,
        emailCheck,
        Zfb_reg_shifou_bangding_shouji,
        Zfb_reg_removeMobile0,
        Zfb_reg_removeMobile1,
        Zfb_reg_removeMobile2,
        Zfb_reg_removeMobile3,
        Zfb_reg_shifou_bangding_shouji_before,
        zfb_reg_open_tb_before,
        Tb_reg_v3_member_reg_fill_email,
        Tb_reg_v3_member_reg_fill_email_before,
        Tb_reg_v3_member_reg_email_sent,
        Tb_reg_v3_Member_reg_fill_user_info,
        Tb_reg_v3_member_reg_reg_success,
        Tb_reg_v3_member_fresh_account_management,
        account_reg_complete_complete,
        Tb_reg_v3_account_reg_complete_complete,
        Tb_reg_v3_asset_paymethod_paymethod,
        Tb_reg_v3_account_reg_success,
        Tb_reg_v3_deliver_address,
        Tb_reg_v3_member_reg_fill_mobile_before,
        Tb_reg_v3_member_reg_fill_mobile,




    }

}
