#pragma once
#include "TEXFileOperation.h"
//don't worry about arch. I have precomplied x86 and x64 version of ktexlib(both release and debug version).
namespace ktexlibwrap
{
	//Just using this namespace as usual.
	using System::String;

	public ref struct mipmap
	{
		System::UInt16 width;
		System::UInt16 height;
		System::UInt16 pitch;
		array<System::Byte>^ data;
	};

	public ref struct RGBA
	{
		System::UInt16 width;
		System::UInt16 height;
		System::UInt16 pitch;
		array<System::Byte>^ data;
	};
	
	typedef System::Collections::Generic::List<mipmap^>^ LMipmap;

	public ref struct mKTEXInfo
	{
		unsigned char flags;
		unsigned short mipscount;
		ktexlib::KTEXFileOperation::textyp texturetype;
		ktexlib::KTEXFileOperation::pixfrm pixelformat;
		ktexlib::KTEXFileOperation::platfrm platform;
		inline mKTEXInfo()
		{
			using namespace ktexlib::KTEXFileOperation;
			platform = platfrm::opengl;
			pixelformat = pixfrm::DXT5;
			texturetype = textyp::d1;
			flags = 0;
			mipscount = 0;
		}
	};
	public ref class KTEX
	{
	public:
		inline KTEX()
		{
			native = new ktexlib::KTEXFileOperation::KTEX();
		}
		inline ~KTEX()
		{
			delete native;
		}

		void PushRGBA(RGBA RGBA_array);
		void PushRGBA(RGBA RGBA_array, unsigned int pitch);
		void Convert();
		void LoadKTEX(System::String^ filepath);
		mipmap^ GetMipmapByPitch(unsigned int pitch);
		mipmap^ GetMipmap(size_t order);
		System::Collections::Generic::List<mipmap^>^ GetMipmaps();
		RGBA^ GetImageFromMipmap(size_t order);
		RGBA^ GetImageArray(unsigned int pitch);
		void clear();
		void operator+=(RGBA src);
		void operator[](int order);
	private:
		ktexlib::KTEXFileOperation::KTEX* native;
	};
}

