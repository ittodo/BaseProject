import std.core;
export module test;

export void MyFunc()
{

}

export template <typename T>
T CallDummy(T a)
{
	return a;
}

namespace assef
{
	export void testFunc()
	{
		std::cout << "Hello World!\n";
	}

	export template<typename T>
	void testFunc2(T tt)
	{
		std::cout << "Hello World!\n";
	}
}

