#pragma once

#include <stdarg.h>
#include <string.h>
#include "../pch.h"

#define LOG_TYPE_PAD 7

void* s_console;

bool InitConsole(void);
void Log(const char* cmd, const char* format, ...);
bool DisposeConsole(void);
