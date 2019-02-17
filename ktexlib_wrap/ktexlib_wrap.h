#pragma once
#include "TEXFileOperation.h"
//don't worry about arch. I have precomplied x86 and x64 version of ktexlib(both release and debug version).
namespace ktexlibwrap 
{
	//Just using this namespace as usual.
	using System::String;

	ref struct mipmap
	{
		System::UInt16 width;
		System::UInt16 height;
		System::UInt16 pitch;
		array<System::Byte>^ data;
	};

	public ref class KTEX
	{
	public:
		KTEX();
		~KTEX();
		void ConvertFromPNG();
		void LoadKTEX(String^ FileName);
		void LoadPNG(String^ FileName);
		void SetInfo(ktexlib::KTEXFileOperation::KTEXInfo info);
		ktexlib::KTEXFileOperation::KTEXInfo GetInfo();
		ktexlibwrap::mipmap^ GetMinmapv1();
		//void SetHeader(ktexlib::KTEXFileOperation::KTEXHeader Header);
		array<System::Byte>^ GetRGBAImage();
	private:
		ktexlib::KTEXFileOperation::KTEXFile* theNative;
	};

}

