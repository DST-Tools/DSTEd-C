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
ktexlibwrap::mipmap^ ktexlibwrap::KTEX::GetMinmapv1()
{
	auto native = theNative->Getmipmapv1();
	auto managed = gcnew ktexlibwrap::mipmap;
	managed->height = native.height;
	managed->width = native.width;
	managed->pitch = native.Z;
	for(size_t i = 0; i < native.data.size(); i++)
	{
		auto it = native.data.begin();
		managed->data->Resize(managed->data, native.data.size());
		managed->data[i] = *it;
		it++;
	}
	return managed;
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
