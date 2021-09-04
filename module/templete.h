#pragma once


namespace desctest
{


	//template<typename T>
	//static auto Func(T& t)
	//{
	//	int i = 0;
	//	i++;
	//}
	template<typename T>
	static void Func(T& t)
	{
		t.Open();
	}


	// See http://www.open-std.org/jtc1/sc22/wg21/docs/papers/2015/n4502.pdf.
	template <typename...>
	using void_t = void;

	// Primary template handles all types not supporting the operation.
	template <typename, template <typename> class, typename = void_t<>>
	struct detect : std::false_type {};

	// Specialization recognizes/validates only types supporting the archetype.
	template <typename T, template <typename> class Op>
	struct detect<T, Op, void_t<Op<T>>> : std::true_type {};


    template< typename T>
    struct has_void_foo_no_args_const
    {
        /* SFINAE foo-has-correct-sig :) */
        template<typename A>
        static std::true_type test(void (A::*)() const) {
            return std::true_type();
        }

        /* SFINAE foo-exists :) */
        template <typename A>
        static decltype(test(&A::foo))
            test(decltype(&A::foo), void*) {
            /* foo exists. What about sig? */
            typedef decltype(test(&A::foo)) return_type;
            return return_type();
        }

        /* SFINAE game over :( */
        template<typename A>
        static std::false_type test(...) {
            return std::false_type();
        }

        /* This will be either `std::true_type` or `std::false_type` */
        typedef decltype(test<T>(0, 0)) type;

        static const bool value = type::value; /* Which is it? */

        /*  `eval(T const &,std::true_type)`
            delegates to `T::foo()` when `type` == `std::true_type`
        */
        static void eval(T const& t, std::true_type) {
            t.foo();
        }
        /* `eval(...)` is a no-op for otherwise unmatched arguments */
        static void eval(...) {
            // This output for demo purposes. Delete
            std::cout << "T::foo() not called" << std::endl;
        }

        /* `eval(T const & t)` delegates to :-
            - `eval(t,type()` when `type` == `std::true_type`
            - `eval(...)` otherwise
        */
        static void eval(T const& t) {
            eval(t, type());
        }
    };

}