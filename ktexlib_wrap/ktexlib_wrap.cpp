#include "stdafx.h"

#include "ktexlib_wrap.h"

using namespace ktexlibwrap;
void ktexlibwrap::KTEX::PushRGBA(RGBA src)
{
	using namespace ktexlib::KTEXFileOperation;
	RGBAv2 temp =
	{
		src.width,
		src.height,
		src.pitch
	};
	auto data = new char[src.data->LongLength];
	for (size_t i = 0; i < src.data->LongLength; i++)
		*(data + i) = src.data[i];
	temp.data.assign(data, data + src.data->LongLength);
	delete[] data;
	native->PushRGBA(temp);
}

void ktexlibwrap::KTEX::PushRGBA(RGBA src, unsigned int pitch)
{
	using namespace ktexlib::KTEXFileOperation;
	RGBAv2 temp =
	{
		src.width,
		src.height,
		pitch
	};
	auto data = new char[src.data->LongLength];
	for (size_t i = 0; i < src.data->LongLength; i++)
		*(data + i) = src.data[i];
	temp.data.assign(data, data + src.data->LongLength);
	delete[] data;
	native->PushRGBA(temp);
}

void ktexlibwrap::KTEX::Convert()
{
	native->Convert();
}

void ktexlibwrap::KTEX::LoadKTEX(System::String^ I)
{
	auto mwcsname = I->ToCharArray();
	auto wcsname = new wchar_t[mwcsname->LongLength];
	native->LoadKTEX(wcsname);
	delete[] wcsname;
}
//watch if struct mipmapv2 destructs.
mipmap^ ktexlibwrap::KTEX::GetMipmapByPitch(unsigned int pitch)
{
	auto temp = native->GetMipmapByPitch(pitch);
	auto mtemp = gcnew mipmap();
	mtemp->height = temp.height;
	mtemp->width = temp.width;
	mtemp->pitch = pitch;
	array<System::Byte>::Resize(mtemp->data, temp.size);
	for (size_t i = 0; i < temp.size; i++)
		mtemp->data[i] = temp.data[i];
	return mtemp;
}
//watch if struct mipmapv2 destructs.
mipmap^ ktexlibwrap::KTEX::GetMipmap(size_t order)
{
	auto temp = native->GetMipmap(order);
	auto mtemp = gcnew mipmap();
	mtemp->height = temp.height;
	mtemp->width = temp.width;
	mtemp->pitch = temp.pitch;
	array<System::Byte>::Resize(mtemp->data, temp.size);
	for (size_t i = 0; i < temp.size; i++)
		mtemp->data[i] = temp.data[i];
	return mtemp;
}

System::Collections::Generic::List<mipmap^>^ ktexlibwrap::KTEX::GetMipmaps()
{
	 
	
	LMipmap temp;
	for (unsigned short i = 0; i < this->native->Info.mipscount; i++)
	{
		temp->Add(this->GetMipmap(i));
	}
	return temp;
}

RGBA^ ktexlibwrap::KTEX::GetImageFromMipmap(size_t order)
{
	auto mtemp = gcnew RGBA();
	auto temp = native->GetImageFromMipmap(order);
	mtemp->height = temp.height;
	mtemp->width = temp.width;
	mtemp->pitch = temp.pitch;
	array<System::Byte>::Resize(mtemp->data, temp.data.size());
	for (size_t i = 0; i < temp.data.size(); i++)
		mtemp->data[i] = temp.data[i];
	return mtemp;
}

RGBA^ ktexlibwrap::KTEX::GetImageArray(unsigned int pitch)
{
	auto mtemp = gcnew RGBA();
	auto temp = native->GetImageArray(pitch);
	mtemp->height = temp.height;
	mtemp->width = temp.width;
	mtemp->pitch = temp.pitch;
	array<System::Byte>::Resize(mtemp->data, temp.data.size());
	for (size_t i = 0; i < temp.data.size(); i++)
		mtemp->data[i] = temp.data[i];
	return mtemp;
}

void ktexlibwrap::KTEX::clear()
{
	native->clear();
}

void ktexlibwrap::KTEX::operator+=(RGBA src)
{
	using namespace ktexlib::KTEXFileOperation;
	RGBAv2 temp =
	{
		src.width,
		src.height,
		src.pitch
	};
	auto data = new char[src.data->LongLength];
	for (size_t i = 0; i < src.data->LongLength; i++)
		*(data + i) = src.data[i];
	temp.data.assign(data, data + src.data->LongLength);
	delete[] data;
	native->PushRGBA(temp);
}

void ktexlibwrap::KTEX::operator[](int i)
{
	
}
