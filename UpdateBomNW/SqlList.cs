namespace UpdateBomNW
{
    public class SqlList
    {
        private string _result;

        /// <summary>
        /// 根据fmaterialidlist批量更新BOM 净重
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        public string GetUpdate(string listid)
        {
            _result = $@"
                           update t1 set t1.FNUMERATOR=t2.FNETWEIGHT,
						          t1.FBASENUMERATOR=t2.FNETWEIGHT
		                    from T_ENG_BOMCHILD t1
		                    inner join T_ENG_BOM t3 on t1.FID=t3.FID
		                    inner join T_BD_MATERIALBASE t2 on t2.FMATERIALID=t3.FMATERIALID
		                    inner join T_BD_MATERIAL t4 on t4.FMATERIALID=t3.FMATERIALID
		                    inner join T_BD_MATERIAL_L t5 on t4.FMATERIALID=t5.FMATERIALID
		                    where t1.FREPLACEGROUP='1' and t4.F_YTC_ASSISTANT5='571f36cd14afe0' and t5.FNAME not like '%套装%' 
		                    AND t5.FNAME not like '%装%' and t5.FNAME not like '%套%'
                            AND t2.FNETWEIGHT>0.1
		                    AND t4.FMATERIALID IN ({listid})

                            --更新‘密度’; 密度=净重/U订货计价规格
                            UPDATE dbo.T_BD_MATERIAL SET F_YTC_DECIMAL1=X.净重/X.U订货计价规格 
                            FROM dbo.T_BD_MATERIAL A
                            INNER JOIN (
				                            SELECT A.FMATERIALID,B.FNETWEIGHT 净重,A.F_YTC_DECIMAL7 U订货计价规格 
				                            FROM dbo.T_BD_MATERIAL A
				                            INNER JOIN dbo.T_BD_MATERIALBASE B ON A.FMATERIALID=B.FMATERIALID
                                            INNER JOIN dbo.T_BD_MATERIAL_L C ON A.FMATERIALID=C.FMATERIALID And C.FLOCALEID=2052
                                            WHERE A.FMATERIALID IN ({listid})
                                            AND A.F_YTC_DECIMAL7 !=0
                                            AND A.F_YTC_ASSISTANT5='571f36cd14afe0' --物料分组-产成品
                                            AND (C.FNAME not like '%装%' and C.FNAME not like '%套%' AND C.FNAME NOT LIKE '%獒王%' AND C.FNAME NOT LIKE '%彪马%')
                                            AND B.FNETWEIGHT>0.1
			                            )X ON A.FMATERIALID=X.FMATERIALID        

                            --更新‘物料单位换算’中‘单位’为KG对应的‘换算关系’值,将‘净重’更新至此项
                            UPDATE dbo.T_BD_UNITCONVERTRATE SET FCONVERTDENOMINATOR=X.净重,FMODIFYDATE=GETDATE()
                            FROM dbo.T_BD_UNITCONVERTRATE A1
                            INNER JOIN (
				                            SELECT A.FMATERIALID,B.FNETWEIGHT 净重 
				                            FROM dbo.T_BD_MATERIAL A
				                            INNER JOIN dbo.T_BD_MATERIALBASE B ON A.FMATERIALID=B.FMATERIALID
				                            WHERE B.FNETWEIGHT>0.1
				                            AND A.FMATERIALID IN ({listid})
			                            )X ON A1.FMATERIALID=X.FMATERIALID
                            WHERE A1.FCURRENTUNITID='10095'   --必须‘单位’为‘KG’才更新

                           --更新‘物料’-整件净重(F_ytc_Text3) 整件毛重(F_ytc_Text4)
                           UPDATE t1 set t1.F_ytc_Text4=t2.FNETWEIGHT*t1.F_ytc_Decimal3, 
                                         t1.F_ytc_Text3=(t2.FNETWEIGHT+t1.F_ytc_Decimal2+t2.FGROSSWEIGHT)*t1.F_ytc_Decimal3 
                          from T_BD_MATERIAL t1 
                          inner join T_BD_MATERIALBASE t2 on t1.FMATERIALID=t2.FMATERIALID 
                          where t1.F_YTC_ASSISTANT5='571f36cd14afe0' and t1.FNUMBER like '%-ck%'
					      AND T1.FMATERIALID IN ({listid})
                        ";
            return _result;
        }

    }
}
