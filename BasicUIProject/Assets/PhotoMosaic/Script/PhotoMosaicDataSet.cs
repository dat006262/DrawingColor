﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

namespace sunny{
	/// <summary>
	/// An object storing all data to create photographic mosaic effect. 
	/// You are supposed to create this with Tool->PhotoMosaic Wizard, or create this object in runtime.
	/// </summary>
	public class PhotoMosaicDataSet:ScriptableObject {
		[System.Serializable]
		public class TextureData{
			public Texture texture;
			public Color color;
			public Rect uvRect;
		}
		/// <summary>
		/// the ratio for chopping, you shouldn't change this value after adding image
		/// </summary>
		public float _ratio;
		/// <summary>
		/// information of each image
		/// </summary>
		public List<TextureData> textures;
		/// <summary>
		/// init with useCache as true, or call makeCache().
		/// to cache all image in one RenderTexture.
		/// the size of this RenderTexture is based on the device and max 8192
		/// </summary>
		public RenderTexture cache;
		/// <summary>
		/// generated by Tool->PhotoMosaic Wizard
		/// </summary>
		public Texture2D cache2D;

		/// <summary>
		/// generated by Tool->PhotoMosaic Wizard, or generated by makeIndexMap()/makeIndexMapAsync().
		/// you need this if you want to use PhotoMosaicImageEffect
		/// </summary>
		//public Texture3D indexMap;
		public Texture2D indexMap;
		/// <summary>
		/// cache image into one RenderTexture
		/// </summary>
		public bool useCache;
		public enum Randomness{
			NoRandom,
			Random1 = 2,
			Random2 = 3,
			Random3 = 4
		}
		public PhotoMosaicDataSet( bool useCache = false){
			this.useCache = useCache;
			_ratio = 1;
			init();
		}
		public PhotoMosaicDataSet( float ratio ,bool useCache = false){
			this.useCache = useCache;
			_ratio = ratio;
			init();
		}
		private void init(){

			textures = new List<TextureData>();
			if(useCache){
				int gridSize = SystemInfo.maxTextureSize;
				if(gridSize>8192)gridSize=8192;
				cache=new RenderTexture(gridSize,gridSize,8);
				RenderTexture.active = cache;
				GL.Clear(true,true,Color.black);
				RenderTexture.active = null;
			}
		}
	
		/// <summary>
		/// Add texture and calculate needed data.
		/// </summary>
		public TextureData addTexture(Texture2D texture){
			float texRatio = texture.width/(float)texture.height;
			int minSize;
			if(texRatio>_ratio){
				minSize = texture.height;
			}else{
				minSize = texture.width;
			}
			TextureData texData = new TextureData();

			texData.uvRect = new Rect();
			texData.uvRect.xMin = .5f-minSize/(float)texture.width/2;
			texData.uvRect.xMax = .5f+minSize/(float)texture.width/2;
			texData.uvRect.yMin = .5f-minSize/(float)texture.height/2;
			texData.uvRect.yMax = .5f+minSize/(float)texture.height/2;
			Color color = Color.clear;
			int sample = 6;
			for(int i = 0 ;i<sample;i++){
				for(int j = 0 ;j<sample;j++){

					color+=texture.GetPixelBilinear(
						Mathf.Lerp(texData.uvRect.xMin,texData.uvRect.xMax, (i+.5f)/sample),
						Mathf.Lerp(texData.uvRect.yMin,texData.uvRect.yMax, (j+.5f)/sample)
					);
				}
			}
			texData.color = color/(sample*sample);
			if(useCache){
				texData.texture = cache;
				int gridSize = cache.width/16;
				RenderTexture.active = cache;
				GL.PushMatrix();
				GL.LoadPixelMatrix(0,cache.width,cache.height,0);

				Graphics.DrawTexture(
					new Rect(gridSize*(textures.Count%16),cache.height-gridSize*(textures.Count/16+1),gridSize,gridSize),
					texture,
					texData.uvRect,
					0,0,0,0);

				GL.PopMatrix();

				texData.uvRect = new Rect();
				texData.uvRect.xMin = (textures.Count%16)/16f;
				texData.uvRect.xMax = (textures.Count%16+1)/16f;
				texData.uvRect.yMin = (textures.Count/16)/16f;
				texData.uvRect.yMax = (textures.Count/16+1)/16f;

				RenderTexture.active = null;

			}else{
				texData.texture = texture;
			}
			textures.Add(texData);
			return texData;
		}
		private class SortItem{
			public Color32 color;
			public byte index;
			public float distance;
		}
		/// <summary>
		/// if useCache is not set to true. you can call this to cache all image to one RenderTexture
		/// </summary>
		public void makeCache(){
			useCache = true;
			int size = SystemInfo.maxTextureSize;
			if(size>8192)size=8192;
			cache=new RenderTexture(size,size,8);
			for(int i = 0 ;i<textures.Count;i++){
				TextureData texData = textures[i];
				Texture2D texture = (Texture2D)texData.texture ;
				texData.texture = cache;
				int gridSize = cache.width/16;
				RenderTexture.active = cache;
				GL.PushMatrix();
				GL.LoadPixelMatrix(0,cache.width,cache.height,0);

				Graphics.DrawTexture(
					new Rect(gridSize*(textures.Count%16),cache.height-gridSize*(textures.Count/16+1),gridSize,gridSize),
					texture,
					texData.uvRect,
					0,0,0,0);

				GL.PopMatrix();

				texData.uvRect = new Rect();
				texData.uvRect.xMin = (textures.Count%16)/16f;
				texData.uvRect.xMax = (textures.Count%16+1)/16f;
				texData.uvRect.yMin = (textures.Count/16)/16f;
				texData.uvRect.yMax = (textures.Count/16+1)/16f;

				RenderTexture.active = null;

			}
		}
		/// <summary>
		/// create a color->image cache for real time PhotoMosaic image effect
		/// </summary>
		/// <param name="sizePow">Size pow.</param>
		public void makeIndexMap(int sizePow = 3){
			if(cache == null){
				makeCache();
			}
			int size = Mathf.RoundToInt(Mathf.Pow(2,sizePow));
			int shift = 8 - sizePow;
			indexMap = new Texture2D(size*size,size,TextureFormat.ARGB32,false);
			indexMap.filterMode = FilterMode.Point;
			indexMap.wrapMode = TextureWrapMode.Clamp;


			ThreadObject tObj = new ThreadObject();
			tObj.textures = textures;
			tObj.makeIndexMapHeavyWork(size,shift);
			indexMap.SetPixels32(tObj.pixels);

			//indexMap.SetPixels32(makeIndexMapHeavyWork(size,shift));
			indexMap.Apply();
		}
		/*
		Color32[] makeIndexMapHeavyWork(int size,int shift ){

			List<SortItem> sortList = new List<SortItem>();
			for(byte i = 0 ;i<textures.Count;i++){
				sortList.Add(new SortItem(){
					color=(Color32)textures[i].color,
					index=i});
			}
			Color32[] pixels = new Color32[size*size*size];
			int index = 0;
			for (byte z = 0; z < size; ++z)
			{
				for (byte y = 0; y < size; ++y)
				{
					for (byte x = 0; x < size; ++x)
					{
						Color32 c= new Color32((byte)(x<<shift),(byte)(y<<shift),(byte)(z<<shift),0);

						for(int i = 0 ;i<sortList.Count;i++){	
							Color32 c2 = sortList[i].color;
							sortList[i].distance = Vector3.Distance(
								new Vector3(c.r,c.g,c.b),
								new Vector3(c2.r,c2.g,c2.b)
							);
						}
						sortList.Sort( delegate(SortItem a, SortItem b) {
							return (a.distance ).CompareTo( b.distance  );
						});


						c.r = sortList[0].index;
						c.g = sortList[1].index;
						c.b = sortList[2].index;
						c.a =sortList[3].index;


						pixels[index] = c;
						index ++;
					}
				}
			}
			return pixels;
		}
		*/

		public class ThreadObject{

			public List<TextureData> textures ;
			public Color32[] pixels ;
			public bool isDone;
			public void makeIndexMapHeavyWork(int size,int shift ){

				List<SortItem> sortList = new List<SortItem>();
				for(byte i = 0 ;i<textures.Count;i++){
					sortList.Add(new SortItem(){
						color=(Color32)textures[i].color,
						index=i});
				}
				pixels = new Color32[size*size*size];
				int index = 0;
				for (byte z = 0; z < size; ++z)
				{
					for (byte y = 0; y < size; ++y)
					{
						for (byte x = 0; x < size; ++x)
						{
							Color32 c= new Color32((byte)(x<<shift),(byte)(y<<shift),(byte)(z<<shift),0);

							for(int i = 0 ;i<sortList.Count;i++){	
								Color32 c2 = sortList[i].color;
								sortList[i].distance = Vector3.Distance(
									new Vector3(c.r,c.g,c.b),
									new Vector3(c2.r,c2.g,c2.b)
								);
							}
							sortList.Sort( delegate(SortItem a, SortItem b) {
								return (a.distance ).CompareTo( b.distance  );
							});


							c.r = sortList[0].index;
							c.g =  sortList[1].index;
							c.b =  sortList[2].index;
							c.a =sortList[3].index;


							pixels[index] = c;
							index ++;
						}
					}
				}
				isDone = true;
			}
		}

		/// <summary>
		/// simiular to makeIndexMap() but async(it costs some time to do this progress).
		/// you should use StartCoroutine() to call this function
		/// </summary>
		public IEnumerator makeIndexMapAsync(int sizePow = 3,Action onComplete = null){
			
			int size = Mathf.RoundToInt(Mathf.Pow(2,sizePow));
			int shift = 8 - sizePow;
			//indexMap = new Texture3D(size,size,size,TextureFormat.ARGB32,false);
			indexMap = new Texture2D(size*size,size,TextureFormat.ARGB32,false);
			indexMap.filterMode = FilterMode.Point;
			indexMap.wrapMode = TextureWrapMode.Clamp;

			ThreadObject tObj = new ThreadObject();
			tObj.textures = textures;
				
			Thread thread = new Thread(delegate() { 
				tObj.makeIndexMapHeavyWork(size,shift);
			});
			thread.Start();
			if(cache == null){
				makeCache();
			}
			while(!tObj.isDone){
				yield return true;
			}
			indexMap.SetPixels32(tObj.pixels);
			indexMap.Apply();
			if(onComplete!= null)onComplete.Invoke();

		}
		/// <summary>
		/// for real time PhotoMosaic image effect, you need more data cached to do that.
		/// call makeCache() and makeIndexMap() to do that.
		/// </summary>
		/// <returns><c>true</c>, if ready for image effect was ised, <c>false</c> otherwise.</returns>
		public bool isReadyForImageEffect(){
			return (cache != null || cache2D != null) && indexMap != null ;
		} 
		void OnDestroy() {
			if(cache)cache.Release();
		}
	}
}