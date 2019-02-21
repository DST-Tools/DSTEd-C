#pragma once
//已经相当于祖传代码了，年代久远
#include <string>
#include <fstream>
#include <vector>
#include <iostream>

#include <filesystem>
#include <exception>
//多线程控制台输出
#ifdef MULTI_THREAD_KTEXCONOUTPUT
#include <mutex>
#endif
namespace ktexlib
{
	namespace KTEXFileOperation
	{
		enum class  platfrm//platform
		{
			opengl = 12,
			xb360 = 11,
			ps3 = 10,
			unk = 0
		};
		enum class pixfrm //pixel form
		{
			 ARGB = 4,
			 DXT1 = 0,
			 DXT3 = 1,
			 DXT5 = 2,
			 unk = 7
		};
		enum class textyp //texture type
		{
			d1 = 1,//1d
			d2 = 2,//2d
			d3 = 3,//3d
			cube = 4//cubemap
		};
		typedef std::vector<unsigned char> uc_vector;

		class KTEXexception :public std::exception
		{
		public:
			~KTEXexception() noexcept
			{
				delete[] msg;
			}
			KTEXexception(const char* msg ,int code) noexcept
			{
				this->msg = msg;
				this->_code = code;
			}
			char const* what() const noexcept
			{
				return msg;
			}
			int const code() noexcept
			{
				return _code;
			}
		private:
			char const* msg;
			int _code;
		};

		struct KTEXHeader
		{
			//CC4
			unsigned int cc4 = 0x5845544B;
			//第一数据块
			unsigned int firstblock = 0;
			//0xFFF 12bit, flags 2bit, mipscount 5bit, textype 4bit
			//pixelformat 5bit, platform 4bit
		};
		struct KTEXInfo
		{
			unsigned char flags = 0;
			unsigned short mipscount = 0;
			textyp texturetype = textyp::d1;
		 	pixfrm pixelformat = pixfrm::DXT5;
			platfrm platform = platfrm::opengl;
		};

		struct mipmap
		{
			unsigned short width = 0;
			unsigned short height = 0;
			unsigned short pitch = 1;//Z Axis
			uc_vector data;
		};

		struct RGBAv2
		{
			unsigned short width=0;
			unsigned short height=0;
			unsigned int pitch=0;
			uc_vector data;
		};

		struct mipmapv2
		{
			unsigned short width =0;
			unsigned short height=0;
			unsigned short pitch = 0;
			unsigned int size = 0;
			char* data = nullptr;
			~mipmapv2();
		};
		
		typedef std::vector<mipmapv2> mipmaps;
		typedef std::vector<RGBAv2> imgs;
		class KTEX
		{
		public:
			void PushRGBA(RGBAv2 RGBA_array);
			void PushRGBA(RGBAv2 RGBA_array, unsigned int pitch);
			void Convert();
			void LoadKTEX(std::experimental::filesystem::path filepath);
			mipmapv2 GetMipmapByPitch(unsigned int pitch);
			mipmapv2 GetMipmap(size_t order);
			RGBAv2 GetImageFromMipmap(size_t order);
			RGBAv2 GetImageArray(unsigned int pitch);
			void clear();
			KTEX();
			~KTEX();
			friend void KTEX2PNG(KTEX target);
			void operator+=(RGBAv2 src);

			KTEXInfo Info;
			std::wstring output;
		private:
			mipmaps mipmaps;
			KTEXHeader Header;
			imgs RGBA_vectors;
		};
		KTEX operator+(KTEX dest, RGBAv2 src);
		void KTEX2PNG(KTEX target);
	}
}