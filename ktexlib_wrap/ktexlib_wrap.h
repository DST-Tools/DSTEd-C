#pragma once
#include "TEXFileOperation.h"

namespace ktexlibwrap 
{
	//Just using this namespace as usual.
	//using namespace System;
	using System::String;
	public ref class KTEX
	{
	public:
		KTEX();
		~KTEX();
		void ConvertFromPNG();
		void LoadKTEX(String^ FileName);
		void LoadPNG(String^ FileName);
		void SetInfo(ktexlib::KTEXFileOperation::KTEXInfo info);
		void SetHeader(ktexlib::KTEXFileOperation::KTEXHeader Header);
		array<System::Byte>^ GetRGBAImage();
	private:
		ktexlib::KTEXFileOperation::KTEXFile* theNative;
	};

}

