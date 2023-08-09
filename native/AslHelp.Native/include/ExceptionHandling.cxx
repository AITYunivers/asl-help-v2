#pragma once

static void vectored_try_catch(void (*func)(), void (*catch_func)())
{
    __try
    {
        func();
    }
    __except (EXCEPTION_EXECUTE_HANDLER)
    {
        catch_func();
    }
}
