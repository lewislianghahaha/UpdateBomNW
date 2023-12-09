using System;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.List.PlugIn;

namespace UpdateBomNW
{
    public class ButtonEvents:AbstractListPlugIn
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            var generate = new Generate();

            //定义主键变量(用于收集所选中的行主键值)
            var fmaterialidlist = string.Empty;

            base.BarItemClick(e);

            if (e.BarItemKey == "tbUpdateBomNw")
            {
                //获取当前登录用户
                var username = this.Context.UserName;

                //todo:只有指定人员才能执行
                if (username != "刘笑梅")
                {
                    View.ShowErrMessage($"用户:'{username}'没有权限,不能执行此操作");
                }
                else
                {
                    //获取列表上通过复选框勾选的记录
                    var selectedrows = this.ListView.SelectedRowsInfo;
                    //判断若没有选择记录,即报出异常提示
                    if (selectedrows.Count == 0)
                    {
                        View.ShowErrMessage("没有选择记录,不能进行更新");
                    }
                    else
                    {
                        //通过循环将选中行的主键进行收集
                        foreach (var row in selectedrows)
                        {
                            if (string.IsNullOrEmpty(fmaterialidlist))
                            {
                                fmaterialidlist = "'" + Convert.ToString(row.PrimaryKeyValue) + "'";
                            }
                            else
                            {
                                fmaterialidlist += "," + "'" + Convert.ToString(row.PrimaryKeyValue) + "'";
                            }
                        }
                        //根据所获取的单号进行更新
                        generate.UpdateBomNw(fmaterialidlist);

                        //输出
                        View.ShowMessage("更新已完成");
                    }
                }
            }

        }
    }
}
