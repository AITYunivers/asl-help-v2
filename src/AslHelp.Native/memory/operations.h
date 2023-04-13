#pragma once

#include "../pch.h"
#include "../io/pipe.h"

PipeResponseCode Deref(iptr* result);

void OpDeref();
void OpRead();
void OpWrite();
