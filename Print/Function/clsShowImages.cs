using System;
using System.Collections ;
namespace Print.Function
{
	/// <summary>
	/// clsShowImages 的摘要说明。
	/// </summary>
	public class clsShowImages
	{
		public clsShowImages()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		public void ShowImages()
		{
			//ESRI.ArcGIS.Geometry.IEnvelope m_IEnvelopeMapCurrentExtent= this.axMapControl.ActiveView.Extent;
			//			double m_dbWidth;
			//			double m_dbHeight;
			double m_dbScale;
			//			m_dbWidth=m_IEnvelopeMapCurrentExtent.Width ;
			//			m_dbHeight=m_IEnvelopeMapCurrentExtent.Height  ;
			m_dbScale=this.axMapControl.MapScale  ;

			IFeature XianShifeature;
			IFeatureLayer iLayerTufu;
			ArrayList TufuResult = new ArrayList() ;
			ArrayList m_ArrayListTFBH=new ArrayList();
			string[] m_strTFBH;

			XianShifeature =null;
			iLayerTufu = null;
			//			XianShifeature =GetFeature(strFeatureClassName[2],"XZQM",selName);
			iLayerTufu = this.getFeatureLayerByName("SDE.FFTC10000");


			//IFeatureLayer iLayerTufu = this.getFeatureLayerByName(strFeatureClassName[3]);
			//进行叠加
			if( iLayerTufu != null)
			{
				TufuResult.Clear();
				//获得当前窗口范围内的图幅
				TufuResult = Overlay1(XianShifeature,iLayerTufu);

				//数据库中是否存在该图幅得影像
				m_ArrayListTFBH=GetInDataBase(TufuResult);

				//根据比例尺确定显示哪些图像

				m_strTFBH=GetShowImageName(m_ArrayListTFBH,m_dbScale);
				if (m_strTFBH==null || m_strTFBH.Length  <1) return;
				LoadImage(m_strTFBH);
					
			}

			TufuResult=null;
			m_ArrayListTFBH=null;
			m_strTFBH=null;

		}

		/// <summary>
		/// 根据比例来获得显示的地图影像文件的名称
		/// </summary>
		/// <param name="p_TufuResult"></param>
		/// <param name="p_dbMapScale"></param>
		/// <returns></returns>
		public string[]  GetShowImageName(System.Collections.ArrayList p_ArrayListTFBH,double p_dbMapScale)
		{
			string m_strJB="";
			string[] m_strTFBH=new string[p_ArrayListTFBH.Count  ];
			if (p_dbMapScale>3000000)
			{
				m_strJB="_5";
			}
			else if  (p_dbMapScale<3000000 && p_dbMapScale>=1000000)
			{
				m_strJB="_4";
			}
			else if  (p_dbMapScale<1000000 && p_dbMapScale>=100000)
			{
				m_strJB="_3";
			}
			else if  (p_dbMapScale<100000 && p_dbMapScale>=50000)
			{
				m_strJB="_2";
			}
			else if  (p_dbMapScale<50000 && p_dbMapScale>12000)
			{
				m_strJB="_1";
			}
			else
			{
				m_strJB="";
			}
			for (int i=0;i<p_ArrayListTFBH.Count   ;i++)
			{
				m_strTFBH[i]=p_ArrayListTFBH[i].ToString()+m_strJB;
				m_strTFBH[i]=m_strTFBH[i].Replace("(","");
				m_strTFBH[i]=m_strTFBH[i].Replace(")","");
				m_strTFBH[i]=m_strTFBH[i].Replace("-","_");
			}
			return m_strTFBH;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p_strTFBH"></param>
		private void LoadImage(string[] p_strTFBH)
		{
			//首先将当前的影像文件的名称与地图中的图层对比，如果该图层（遥感影像）不在该数组中，则将其的设为不可见，如果在则将其设为可见
			//如果数组中的影像还没有打开，则到数据库中打开该文件
			int m_IntHasShowImage=0;
			ILayer fLayer=null;
			bool m_IsShow=false;
			int[] p_intIsShow=new int[p_strTFBH.Length ] ;
			ArrayList arrayOflayers=mapFuntion.getAllLayers();				//得到图层数组
			string m_strLayerName="";
			string m_strTempLayerName="";
		
			IEnumerator myEnumerator = arrayOflayers.GetEnumerator();		//遍历图层数组
			while ( myEnumerator.MoveNext() )
			{///遍历图层决定已经显示的图像是否显示
				fLayer=(ILayer)myEnumerator.Current;
				m_IsShow=false;
				if(fLayer!= null)
				{
					m_strLayerName=fLayer.Name.ToUpper();
					for (int i=0;i<p_strTFBH.Length ;i++)
					{//判断图层是否在数组中
						if (m_strLayerName.Length-p_strTFBH[i].Length<0) continue;
						m_strTempLayerName=m_strLayerName.Substring(m_strLayerName.Length-p_strTFBH[i].Length, p_strTFBH[i].Length);
						
						if(m_strTempLayerName.ToUpper().Equals(p_strTFBH[i].ToUpper()))
						{				
							p_intIsShow[i]=1;//该影像已经存在,不需要再次下载
							m_IsShow=true;//该影像需要显示
							m_IntHasShowImage=m_IntHasShowImage+1;
							break;
						}
					}
					if(m_IsShow==true)
					{
						if (fLayer.Visible==false)
							fLayer.Visible=true;
					}
					else
					{
						if (fLayer is RasterLayer )
						{
							if (fLayer.Visible==true)
								fLayer.Visible=false;
						}
					}
				
				}
			}
		
			if (m_IntHasShowImage==p_strTFBH.Length ) return;//所有的影像都已经显示
			//将还没有显示的图像读入当前地图
			IRasterWorkspaceEx m_IRasterWorkspaceEx=mapFuntion.OpenIRasterWorkspaceEx(m_DataAccess.Server,m_DataAccess.Instance,
				m_DataAccess.DataBase,m_DataAccess.UserID,m_DataAccess.Password, "sde.DEFAULT");
			IRasterCatalog m_IRasterCatalog=mapFuntion.openRasterCatalog(m_IRasterWorkspaceEx,"SDE.Raser2005");

			for (int j=0;j<p_strTFBH.Length;j++)
			{
				if (p_intIsShow[j]!=1)
				{
					OpenImageFile(m_IRasterCatalog,p_strTFBH[j]);
				}
				else
				{
				}
			}
			this.axMapControl.Refresh();
		}

		/// <summary>
		/// 根据重叠的图幅编号查数据库中是否存在影像数据
		/// </summary>
		/// <returns></returns>
		public System.Collections.ArrayList GetInDataBase(System.Collections.ArrayList p_LayerNames)
		{
			ArrayList m_ArrayListImages=new ArrayList();
			string[] m_strImagesTemp;
			bool got = false;
			//			XzqTreeView m_XTV = new XzqTreeView() ;
			int sum = p_LayerNames.Count;

			int m_intTemp=-1;
			int m_intTemp0=0;
			string m_TFBH = "";

			try                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
			{
				//在数据库查询时查询条件不能超过1000个，因此需要对他们进行分组来查询，然后将查询后的结果再合并到一个数组中				
				for (int k = 0; k<p_LayerNames.Count; k++)
				{
					m_intTemp=m_intTemp+1;	
					
					if (((k %999==0) && k!=0) || k==p_LayerNames.Count-1 || (k==0 && p_LayerNames.Count==1))
					{
							
						m_TFBH = m_TFBH + "'" + p_LayerNames[k].ToString()+ "'";
						//							m_intTemp0=m_intTemp0+1
						m_intTemp=-1;
					}
					else
					{
						m_TFBH = m_TFBH + "'" + p_LayerNames[k].ToString() + "',";
					}
					if (m_intTemp!=-1  )
					{							
						continue;
					}
					else
					{
						m_strImagesTemp=ReadImagesTFBH(m_TFBH);
						if (m_strImagesTemp==null || m_strImagesTemp.Length <1) 
						{
							m_TFBH =  "";		
							continue;
						}
						for (int m=0;m<m_strImagesTemp.Length ;m++)
						{
							m_ArrayListImages.Add((object)m_strImagesTemp[m]);
						}
					}							
					
					m_TFBH =  "";					
				}
				return m_ArrayListImages;
			}
			catch(System.Exception errs)
			{
				System.Windows.Forms.MessageBox.Show(this,errs.Message,"错误提示",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
			}
			
			return null;
		}

		private string[] ReadImagesTFBH(string p_TFBH)
		{
			string[] m_strImagesTemp;
			System.Data.DataRowCollection m_DataRowCollection;
			string strSQL = "SELECT SATELITE,SENSOR,SENSORMODE,CLOUDNUMB,SUNAZIMUTH,SUNELEVATION,DOWNORUP,ORBITNUM,SIDEANGLE,ULLATITUDE,ULLONGITUDE,URLATITUDE,URLONGITUDE,LRLATITUDE,LRLONGITUDE,LLLATITUDE,LLLONGITUDE,COLUMNID,HROWID,QCQUIREDATE,RECEIVINGSTATION,PRODUCTGRADE,QUALITY,PRODUCEDATE,PRODUCER,LOADINTIME,LOADINSTAFFNAME,TFBH,LAYERNAME,RASTERCATALOG FROM YGYX WHERE TFBH IN ( " + p_TFBH.ToUpper() + ") or TFBH IN ( " + p_TFBH.ToLower() + ")";
				
			
			try
			{
				m_DataRowCollection=objectDataAccess.getDataRowsByQueryString(strSQL);
				if (m_DataRowCollection==null) return null;

				m_strImagesTemp=new string[ m_DataRowCollection.Count] ;
				if (m_DataRowCollection!=null && m_DataRowCollection.Count>0)
				{
					//						FillListView1(m_DataRowCollection);
					for (int i=0;i<m_DataRowCollection.Count ;i++)
					{
						m_strImagesTemp[i]=m_DataRowCollection[i]["TFBH"].ToString();
					}					
				}
				else 
				{
					MessageBox.Show("该行政区没有影像");					
				}
				return m_strImagesTemp;
					
			}
			catch(System.Exception errs)
			{
				System.Windows.Forms.MessageBox.Show(this,errs.Message,"错误提示",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
			}
			return null;
			
		}

		

		
		/// 加入查询到的栅格影像（数组）
		/// </summary>
		/// <param name="p_LayerNanes"></param>
		private void OpenImageFile(ESRI.ArcGIS.Geodatabase.IRasterCatalog m_IRasterCatalog,string  p_LayerName)
		{
			
			try
			{
				
				string layerName = p_LayerName;
				//					layerName = "SDE."+layerName ;
				//MessageBox.Show(layerName); 
				//					IRasterDataset iRasterDataset = this.mapFuntion.OpenArcSDE(m_DataAccess.Server,m_DataAccess.Instance,
				//						m_DataAccess.DataBase,m_DataAccess.UserID,
				//						m_DataAccess.Password,"sde.DEFAULT",layerName); 
				
				IRasterDataset m_IRasterDataset= mapFuntion.GetRasterDatasetFromRasterCatalog(m_IRasterCatalog,p_LayerName);
				if (m_IRasterDataset==null) return;				

				if(mapFuntion.haveRasterDataset(layerName)==false)
					//MessageBox.Show("名为"+layerName+"的图幅已经加入"); 
					
				{
					this.mapFuntion.AddRasterLayer(this.axMapControl,m_IRasterDataset);
					//						//pageLayoutContrl 加入栅格图层
					//						this.CopyAndOverwriteMap22();
					//						//地图范围保持一致
					//						m_bUpdateFocusMap = true;
					//						//this.CopyAndOverwriteMap();
		
				}
				
			}
			catch (Exception e)
			{
				Debug.WriteLine("读入影像出错："+e.Message );
			}
		}

		public ESRI.ArcGIS.Carto.IFeatureLayer getFeatureLayerByName(String layerName)
		{
			IFeatureLayer fLayer=null;
			IFeatureLayer resOfLayer=null;
			try 
			{
				ArrayList arrayOflayers=this.getFeatureLayers();				//得到图层数组
		
				IEnumerator myEnumerator = arrayOflayers.GetEnumerator();		//遍历图层数组
				while ( myEnumerator.MoveNext() )
				{
					fLayer=(IFeatureLayer)myEnumerator.Current;
					if(fLayer!= null)
					{
						if(fLayer.Name.Equals(layerName))
						{
							resOfLayer= fLayer;
							break;
						}	
					}
				}		
			} 		
			catch (Exception e) 
			{
				Debug.WriteLine("—— 得到所选的layer——："+e.Message );
			}
			return resOfLayer;	
		}

		/// <summary>
		///  获取所有的矢量图层，返回矢量图层数组，不包括raster影像
		/// </summary>
		/// <returns></returns>
		public System.Collections.ArrayList getFeatureLayers()
		{
			ILayer qrylayer = null;
			FeatureLayer qryFeatLayer = null;
			ArrayList arrOfFeatlayer=new ArrayList();
			int layerCount;													//图层数
			try 
			{
				layerCount = this.axMapControl.LayerCount;
				for(int i=0; i<layerCount; i++)
				{
					qrylayer = this.axMapControl.get_Layer(i);
					//判断qrylayer是否为FeatureLayer类型
					if(!qrylayer.Name.Equals(null) && (qrylayer is FeatureLayer))
					{
						qryFeatLayer = (FeatureLayer)qrylayer;
						arrOfFeatlayer.Add(qryFeatLayer) ;						
					}
				}		
			}
			catch (Exception e) 
			{
				Debug.WriteLine("——获取所有的图层数组(不包括raster)出错——："+e.Message );
			}
			return arrOfFeatlayer;
		}

		/// <summary>
		/// 叠加分析
		/// </summary>
		/// <param name="mfeature"></param>
		/// <param name="mLayer"></param>
		private System.Collections.ArrayList Overlay1(ESRI.ArcGIS.Geodatabase.IFeature mfeature,ESRI.ArcGIS.Carto.IFeatureLayer mLayer)
		{
			try
			{
				//				ESRI.ArcGIS.Geometry.IEnvelope m_IEnvelopeMapCurrentExtent= this.axMapControl.ActiveView.Extent;
				//				double m_dbWidth;
				//				double m_dbHeight;
				//				double m_dbScale;
				//				m_dbWidth=m_IEnvelopeMapCurrentExtent.Width ;
				//				m_dbHeight=m_IEnvelopeMapCurrentExtent.Height  ;
				//				m_dbScale=this.axMapControl.MapScale  ;

				//				double m_dbSceneScale =1/this.axSceneControl1.SceneViewer.Camera.;
				ESRI.ArcGIS.Geometry.IEnvelope m_IEnvelope;			
				
				m_IEnvelope=  this.axMapControl.Extent;
				if (axMapControl.CurrentTool!=null)
				{
					string m_strCurrentTool=axMapControl.CurrentTool.ToString();
					if (m_strCurrentTool=="ESRI.ArcGIS.ControlCommands.ControlsMapPanToolClass")
					{
						if (m_IPointStart!=null && m_IPointEnd!=null)
						{
							m_IEnvelope.Offset(m_IPointStart.X-m_IPointEnd.X,m_IPointStart.Y -m_IPointEnd.Y);
						}
					}
				}
				
				//				m_Width=m_IEnvelope.Width;
				//				m_Height=m_IEnvelope.Height ;
				//				ESRI.ArcGIS.Geometry.IGeometry
				IGeometry m_Geometry =(IGeometry)m_IEnvelope;
				


				ISpatialFilter m_SpatialFilter= new SpatialFilterClass();
				m_SpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
				//				IGeometry m_Geometry = mfeature.Shape;
				m_SpatialFilter.Geometry = m_Geometry;
				IFeatureCursor m_FeatureCursor = null; 
				m_FeatureCursor = mLayer.Search(m_SpatialFilter,true);
				 
				//查找图幅号名称
				IFeature selFeature = m_FeatureCursor.NextFeature();
				ArrayList tfbh = new ArrayList(); 
				int i = 0;
				while(selFeature!= null)
				{
					int index =selFeature.Fields.FindField("TFBH");
					string thevalue = selFeature.get_Value(index).ToString();
					Debug.WriteLine(thevalue);
					// IField filed = selFeature.Fields.get_Field(index);
					tfbh.Add(thevalue);
					selFeature = m_FeatureCursor.NextFeature();
					i++;
				}
				return tfbh;

			}
			catch(System.Exception errs)
			{
				throw errs;	
			}
			
		}
	}
}
