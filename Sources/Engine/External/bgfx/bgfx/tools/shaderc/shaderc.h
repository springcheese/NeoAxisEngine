/*
 * Copyright 2011-2022 Branimir Karadzic. All rights reserved.
 * License: https://github.com/bkaradzic/bgfx/blob/master/LICENSE
 */

#ifndef SHADERC_H_HEADER_GUARD
#define SHADERC_H_HEADER_GUARD

namespace bgfx
{
	extern bool g_verbose;
}

//#define _BX_TRACE(_format, ...) \
//				BX_MACRO_BLOCK_BEGIN \
//					if (bgfx::g_verbose) \
//					{ \
//						fprintf(stdout, BX_FILE_LINE_LITERAL "" _format "\n", ##__VA_ARGS__); \
//					} \
//				BX_MACRO_BLOCK_END
//
//#define _BX_WARN(_condition, _format, ...) \
//				BX_MACRO_BLOCK_BEGIN \
//					if (!(_condition) ) \
//					{ \
//						BX_TRACE("WARN " _format, ##__VA_ARGS__); \
//					} \
//				BX_MACRO_BLOCK_END
//
//#define _BX_ASSERT(_condition, _format, ...) \
//				BX_MACRO_BLOCK_BEGIN \
//					if (!(_condition) ) \
//					{ \
//						BX_TRACE("CHECK " _format, ##__VA_ARGS__); \
//						bx::debugBreak(); \
//					} \
//				BX_MACRO_BLOCK_END

#define BX_TRACE _BX_TRACE
#define BX_WARN  _BX_WARN
#define BX_ASSERT _BX_ASSERT

#ifndef SHADERC_CONFIG_HLSL
#	define SHADERC_CONFIG_HLSL BX_PLATFORM_WINDOWS || BX_PLATFORM_WINRT //!!!!betauser
#endif // SHADERC_CONFIG_HLSL

#include <alloca.h>
#include <stdint.h>
#include <string.h>
#include <algorithm>
#include <string>
#include <vector>
#include <unordered_map>

#include <bx/bx.h>
#include <bx/debug.h>
#include <bx/commandline.h>
#include <bx/endian.h>
#include <bx/uint32_t.h>
#include <bx/string.h>
#include <bx/hash.h>
#include <bx/file.h>
#include "../../src/vertexlayout.h"


//!!!!betauser
#define SHADERC_AS_DLL
#ifdef SHADERC_AS_DLL
extern "C" int override_fprintf(FILE* _stream, const char* _format, ...);
extern "C" int override_vfprintf(FILE* _stream, const char* _format, va_list _argList);
#define FPRINTF(strm, fmt, ...) override_fprintf(strm, fmt, ##__VA_ARGS__)
#define VFPRINTF(strm, fmt, argList) override_vfprintf(strm, fmt, argList)
#else
#define FPRINTF(strm, fmt, ...) fprintf(strm, fmt, ##__VA_ARGS__)
#define VFPRINTF(strm, fmt, argList) fprintf(strm, fmt, argList)
#endif



namespace bgfx
{
	extern bool g_verbose;

	class LineReader : public bx::ReaderI
	{
	public:
		LineReader(const char* _str)
			: m_str(_str)
			, m_pos(0)
			, m_size(bx::strLen(_str) )
		{
		}

		virtual int32_t read(void* _data, int32_t _size, bx::Error* _err) override
		{
			if (m_str[m_pos] == '\0'
			||  m_pos == m_size)
			{
				BX_ERROR_SET(_err, bx::kErrorReaderWriterEof, "LineReader: EOF.");
				return 0;
			}

			uint32_t pos = m_pos;
			const char* str = &m_str[pos];
			const char* nl = bx::strFindNl(bx::StringView(str, str + (m_size - pos))).getPtr();
			pos += (uint32_t)(nl - str);

			const char* eol = &m_str[pos];

			uint32_t size = bx::uint32_min(uint32_t(eol - str), _size);

			bx::memCopy(_data, str, size);
			m_pos += size;

			return size;
		}

		const char* m_str;
		uint32_t m_pos;
		uint32_t m_size;
	};

	bx::StringView nextWord(bx::StringView& _parse);

	constexpr uint8_t kUniformFragmentBit  = 0x10;
	constexpr uint8_t kUniformSamplerBit   = 0x20;
	constexpr uint8_t kUniformReadOnlyBit  = 0x40;
	constexpr uint8_t kUniformCompareBit   = 0x80;
	constexpr uint8_t kUniformMask = 0
		| kUniformFragmentBit
		| kUniformSamplerBit
		| kUniformReadOnlyBit
		| kUniformCompareBit
		;

	const char* getUniformTypeName(UniformType::Enum _enum);
	UniformType::Enum nameToUniformTypeEnum(const char* _name);

	struct Uniform
	{
		Uniform()
			: type(UniformType::Count)
			, num(0)
			, regIndex(0)
			, regCount(0)
			, texComponent(0)
			, texDimension(0)
		{
		}

		std::string name;
		UniformType::Enum type;
		uint8_t num;
		uint16_t regIndex;
		uint16_t regCount;
		uint8_t texComponent;
		uint8_t texDimension;
	};

	struct Options
	{
		Options();

		void dump();

		char shaderType;
		std::string platform;
		std::string profile;

		std::string	inputFilePath;
		std::string	outputFilePath;

		std::vector<std::string> includeDirs;
		std::vector<std::string> defines;
		std::vector<std::string> dependencies;

		bool disasm;
		bool raw;
		bool preprocessOnly;
		bool depends;

		bool debugInformation;

		bool avoidFlowControl;
		bool noPreshader;
		bool partialPrecision;
		bool preferFlowControl;
		bool backwardsCompatibility;
		bool warningsAreErrors;
		bool keepIntermediate;

		bool optimize;
		uint32_t optimizationLevel;
	};

	typedef std::vector<Uniform> UniformArray;

	void printCode(const char* _code, int32_t _line = 0, int32_t _start = 0, int32_t _end = INT32_MAX, int32_t _column = -1);
	void strReplace(char* _str, const char* _find, const char* _replace);
	int32_t writef(bx::WriterI* _writer, const char* _format, ...);
	void writeFile(const char* _filePath, const void* _data, int32_t _size);

	bool compileGLSLShader(const Options& _options, uint32_t _version, const std::string& _code, bx::WriterI* _writer);
	bool compileHLSLShader(const Options& _options, uint32_t _version, const std::string& _code, bx::WriterI* _writer);
	bool compileMetalShader(const Options& _options, uint32_t _version, const std::string& _code, bx::WriterI* _writer);
	bool compilePSSLShader(const Options& _options, uint32_t _version, const std::string& _code, bx::WriterI* _writer);
	bool compileSPIRVShader(const Options& _options, uint32_t _version, const std::string& _code, bx::WriterI* _writer);

	const char* getPsslPreamble();

} // namespace bgfx

#endif // SHADERC_H_HEADER_GUARD
