#include "stdafx.h"

#include "ktexlib_wrap.h"

ktexlibwrap::KTEX::KTEX()
{
	this->theNative = new ktexlib::KTEXFileOperation::KTEXFile();
}

ktexlibwrap::KTEX::~KTEX()
{
	delete this->theNative;
}

void ktexlibwrap::KTEX::ConvertFromPNG()
{
	this->theNative->ConvertFromPNG();
}

void ktexlibwrap::KTEX::LoadKTEX(String ^ FileName)
{
	auto a = FileName->ToCharArray();
	this->theNative->LoadKTEX(std::wstring(a[1], a[a->Length]));
}

void ktexlibwrap::KTEX::LoadPNG(String ^ FileName)
{
	auto managedwfilename = FileName->ToCharArray();
	wchar_t* wfilename = new wchar_t[managedwfilename->LongLength];
	for (size_t i = 0; i < managedwfilename->LongLength; i++)
	{
		wfilename[i] = managedwfilename[i];
	}
	char* mbfilename = new char[(managedwfilename->LongLength) * 2];
	size_t convertedcount = 0;
	wcstombs_s(&convertedcount,mbfilename, (managedwfilename->LongLength) * 2, wfilename, (size_t)managedwfilename->LongLength);
	this->theNative->LoadPNG(mbfilename);
	delete[] mbfilename;
	delete[] wfilename;
}

void ktexlibwrap::KTEX::SetInfo(ktexlib::KTEXFileOperation::KTEXInfo info)
{
	this->theNative->Info = info;
}

ktexlib::KTEXFileOperation::KTEXInfo ktexlibwrap::KTEX::GetInfo()
{
	return theNative->Info;
}
ktexlib::KTEXFileOperation::mipmap ktexlibwrap::KTEX::GetMinmapv1()
{
	return theNative->Getmipmapv1();
}
/*
void ktexlibwrap::KTEX::SetHeader(ktexlib::KTEXFileOperation::KTEXHeader Header)
{
	this->theNative->Header = Header;
}
*/
array<System::Byte>^ ktexlibwrap::KTEX::GetRGBAImage()
{
	ktexlib::KTEXFileOperation::uc_vector ret;
	array<System::Byte>^ retarray = nullptr;
	this->theNative->GetRBGAImage(ret);
	retarray = gcnew array<System::Byte>(ret.size());
	for (auto it = ret.begin(); it != ret.end(); it++)
	{
		unsigned int i = 0;
		retarray[i] = *it;
		i++;
	}
	return retarray;
}
