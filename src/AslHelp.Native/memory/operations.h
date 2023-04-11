#pragma once

#include "../pch.h"
#include "../io/pipe.h"

PipeResponseCode Deref(iptr* result);
PipeResponseCode ReadValue(iptr* result, i32* size);
