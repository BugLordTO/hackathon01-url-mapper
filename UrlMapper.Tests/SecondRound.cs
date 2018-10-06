using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace UrlMapper.Tests
{
    public class SecondRound
    {
        [Theory(DisplayName = "ระบบสามารถทำการแยกข้อมูลออกจาก url ที่ส่งเข้ามาในรูปแบบต่างๆได้ถูกต้อง")]
        [InlineData(null, "", "", "")]
        [InlineData(null, " ", "", "")]
        [InlineData(null, "something", "", "")]
        [InlineData(null, null, "", "")]
        [InlineData("", "", "", "")]
        [InlineData("", " ", "", "")]
        [InlineData("", "something", "", "")]
        [InlineData("", null, "", "")]
        [InlineData(" ", "", "", "")]
        [InlineData(" ", " ", "", "")]
        [InlineData(" ", "something", "", "")]
        [InlineData(" ", null, "", "")]
        [InlineData("something", "", "", "")]
        [InlineData("something", " ", "", "")]
        [InlineData("something", "something", "", "")]
        [InlineData("something", null, "", "")]

        [InlineData("something/{p1}", "something/miolynet", "{p1}", "miolynet")]
        [InlineData("something/aa{p1-with-prefix}", "something/aamiolynet", "{p1-with-prefix}", "miolynet")]
        [InlineData("something/{p1-with-postfix}aa", "something/miolynetaa", "{p1-with-postfix}", "miolynet")]
        [InlineData("something/aa{p1-with-prefix-and-postfix}bb", "something/aamiolynetbb", "{p1-with-prefix-and-postfix}", "miolynet")]
        [InlineData("something/aa{p1-no-value}bb", "something/aabb", "{p1-no-value}", "")]
        [InlineData("something/{p1}", "nothing/miolynet", "", "")]

        [InlineData("something/{p1}/{p2}", "something/miolynet/thes", "{p1},{p2}", "miolynet,thes")]
        [InlineData("something/{p1}/aa{p2-with-prefix}", "something/miolynet/aathes", "{p1},{p2-with-prefix}", "miolynet,thes")]
        [InlineData("something/{p1}/{p2-with-postfix}aa", "something/miolynet/thesaa", "{p1},{p2-with-postfix}", "miolynet,thes")]
        [InlineData("something/{p1}/aa{p2-with-prefix-and-postfix}bb", "something/miolynet/aathesbb", "{p1},{p2-with-prefix-and-postfix}", "miolynet,thes")]
        [InlineData("something/{p1}/aa{p2-no-value}bb", "something/miolynet/aathesbb", "{p1},{p2-no-value}", "miolynet,")]

        [InlineData("something/aa{p1-with-prefix}/xx{p2-with-prefix}", "something/aamiolynet/xxthes", "{p1-with-prefix},{p2-with-prefix}", "miolynet,thes")]
        [InlineData("something/aa{p1-with-prefix}/{p2-with-postfix}xx", "something/aamiolynet/thesxx", "{p1-with-prefix},{p2-with-postfix}", "miolynet,thes")]
        [InlineData("something/aa{p1-with-prefix}/xx{p2-with-prefix-and-postfix}yy", "something/aamiolynet/xxthesyy", "{p1-with-prefix},{p2-with-prefix-and-postfix}", "miolynet,thes")]
        [InlineData("something/aa{p1-with-prefix}/xx{p2-no-data}yy", "something/aamiolynet/xxyy", "{p1-with-prefix},{p2-no-data}", "miolynet,")]

        [InlineData("something/{p1-with-postfix}aa/xx{p2-with-prefix}", "something/miolynetaa/xxthes", "{p1-with-postfix},{p2-with-prefix}", "miolynet,thes")]
        [InlineData("something/{p1-with-postfix}aa/{p2-with-postfix}xx", "something/miolynetaa/thesxx", "{p1-with-postfix},{p2-with-postfix}", "miolynet,thes")]
        [InlineData("something/{p1-with-postfix}aa/xx{p2-with-prefix-and-postfix}yy", "something/miolynetaa/xxthesyy", "{p1-with-postfix},{p2-with-prefix-and-postfix}", "miolynet,thes")]
        [InlineData("something/{p1-with-postfix}aa/xx{p2-no-data}yy", "something/miolynetaa/xxyy", "{p1-with-postfix},{p2-no-data}", "miolynet,")]

        [InlineData("something/aa{p1-with-prefix-and-postfix}bb/xx{p2-with-prefix}", "something/aamiolynetbb/xxthes", "{p1-with-prefix-and-postfix},{p2-with-prefix}", "miolynet,thes")]
        [InlineData("something/aa{p1-with-prefix-and-postfix}bb/{p2-with-postfix}xx", "something/aamiolynetbb/thesxx", "{p1-with-prefix-and-postfix},{p2-with-postfix}", "miolynet,thes")]
        [InlineData("something/aa{p1-with-prefix-and-postfix}bb/xx{p2-with-prefix-and-postfix}yy", "something/aamiolynetbb/xxthesyy", "{p1-with-prefix-and-postfix},{p2-with-prefix-and-postfix}", "miolynet,thes")]
        [InlineData("something/aa{p1-with-prefix-and-postfix}bb/xx{p2-no-data}yy", "something/aamiolynetbb/xxyy", "{p1-with-prefix-and-postfix},{p2-no-data}", "miolynet,")]

        [InlineData("something/aa{p1-no-data}bb/xx{p2-with-prefix}", "something/aabb/xxthes", "{p1-with-prefix-and-postfix},{p2-with-prefix}", ",thes")]
        [InlineData("something/aa{p1-no-data}bb/{p2-with-postfix}xx", "something/aabb/thesxx", "{p1-with-prefix-and-postfix},{p2-with-postfix}", ",thes")]
        [InlineData("something/aa{p1-no-data}bb/xx{p2-with-prefix-and-postfix}yy", "something/aabb/xxthesyy", "{p1-with-prefix-and-postfix},{p2-with-prefix-and-postfix}", ",thes")]
        [InlineData("something/aa{p1-no-data}bb/xx{p2-no-data}yy", "something/aabb/xxyy", "{p1-with-prefix-and-postfix},{p2-no-data}", ",")]

        [InlineData("https://hackathon.com", "https://hackathon.com", "", "")]
        [InlineData("https://hackathon.com", "https://hackathon.com/", "", "")]
        [InlineData("https://hackathon.com/", "https://hackathon.com", "", "")]
        [InlineData("https://hackathon.com/", "https://hackathon.com/", "", "")]
        [InlineData("https://hackathon.com/none", "https://hackathon.com/none", "", "")]
        [InlineData("https://hackathon.com/none", "https://hackathon.com/none/", "", "")]
        [InlineData("https://hackathon.com/none/", "https://hackathon.com/none", "", "")]
        [InlineData("https://hackathon.com/none/", "https://hackathon.com/none/", "", "")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com", "{username}", "")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/", "{username}", "")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com//", "{username}", "/")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com///", "{username}", "//")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com////", "{username}", "///")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test123", "{username}", "test123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test!123", "{username}", "test!123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test\"123", "{username}", "test\"123")]

        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test#123", "{username}", "test#123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test$123", "{username}", "test$123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test%123", "{username}", "test%123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test&123", "{username}", "test&123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test'123", "{username}", "test'123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test(123", "{username}", "test(123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test)123", "{username}", "test)123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test*123", "{username}", "test*123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test+123", "{username}", "test+123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test,123", "{username}", "test,123", ",,")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test-123", "{username}", "test-123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test.123", "{username}", "test.123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test/123", "{username}", "test/123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test:123", "{username}", "test:123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test;123", "{username}", "test;123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test<123", "{username}", "test<123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test=123", "{username}", "test=123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test>123", "{username}", "test>123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test?123", "{username}", "test?123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test@123", "{username}", "test@123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test[123", "{username}", "test[123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test\\123", "{username}", "test\\123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test]123", "{username}", "test]123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test^123", "{username}", "test^123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test_123", "{username}", "test_123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test`123", "{username}", "test`123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test|123", "{username}", "test|123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test~123", "{username}", "test~123")]
        [InlineData("https://hackathon.com/{username}", "https://hackathon.com/test 123", "{username}", "test 123")]

        [InlineData("https://hackathon.com/{user#name}", "https://hackathon.com/test#123", "{user#name}", "test#123")]
        [InlineData("https://hackathon.com/{user$name}", "https://hackathon.com/test$123", "{user$name}", "test$123")]
        [InlineData("https://hackathon.com/{user%name}", "https://hackathon.com/test%123", "{user%name}", "test%123")]
        [InlineData("https://hackathon.com/{user&name}", "https://hackathon.com/test&123", "{user&name}", "test&123")]
        [InlineData("https://hackathon.com/{user'name}", "https://hackathon.com/test'123", "{user'name}", "test'123")]
        [InlineData("https://hackathon.com/{user(name}", "https://hackathon.com/test(123", "{user(name}", "test(123")]
        [InlineData("https://hackathon.com/{user)name}", "https://hackathon.com/test)123", "{user)name}", "test)123")]
        [InlineData("https://hackathon.com/{user*name}", "https://hackathon.com/test*123", "{user*name}", "test*123")]
        [InlineData("https://hackathon.com/{user+name}", "https://hackathon.com/test+123", "{user+name}", "test+123")]
        [InlineData("https://hackathon.com/{user,name}", "https://hackathon.com/test,123", "{user,name}", "test,123", ",,")]
        [InlineData("https://hackathon.com/{user-name}", "https://hackathon.com/test-123", "{user-name}", "test-123")]
        [InlineData("https://hackathon.com/{user.name}", "https://hackathon.com/test.123", "{user.name}", "test.123")]
        [InlineData("https://hackathon.com/{user/name}", "https://hackathon.com/test/123", "{user/name}", "test/123")]
        [InlineData("https://hackathon.com/{user:name}", "https://hackathon.com/test:123", "{user:name}", "test:123")]
        [InlineData("https://hackathon.com/{user;name}", "https://hackathon.com/test;123", "{user;name}", "test;123")]
        [InlineData("https://hackathon.com/{user<name}", "https://hackathon.com/test<123", "{user<name}", "test<123")]
        [InlineData("https://hackathon.com/{user=name}", "https://hackathon.com/test=123", "{user=name}", "test=123")]
        [InlineData("https://hackathon.com/{user>name}", "https://hackathon.com/test>123", "{user>name}", "test>123")]
        [InlineData("https://hackathon.com/{user?name}", "https://hackathon.com/test?123", "{user?name}", "test?123")]
        [InlineData("https://hackathon.com/{user@name}", "https://hackathon.com/test@123", "{user@name}", "test@123")]
        [InlineData("https://hackathon.com/{user[name}", "https://hackathon.com/test[123", "{user[name}", "test[123")]
        [InlineData("https://hackathon.com/{user\\name}", "https://hackathon.com/test\\123", "{user\\name}", "test\\123")]
        [InlineData("https://hackathon.com/{user]name}", "https://hackathon.com/test]123", "{user]name}", "test]123")]
        [InlineData("https://hackathon.com/{user^name}", "https://hackathon.com/test^123", "{user^name}", "test^123")]
        [InlineData("https://hackathon.com/{user_name}", "https://hackathon.com/test_123", "{user_name}", "test_123")]
        [InlineData("https://hackathon.com/{user`name}", "https://hackathon.com/test`123", "{user`name}", "test`123")]
        [InlineData("https://hackathon.com/{user|name}", "https://hackathon.com/test|123", "{user|name}", "test|123")]
        [InlineData("https://hackathon.com/{user~name}", "https://hackathon.com/test~123", "{user~name}", "test~123")]
        [InlineData("https://hackathon.com/{user name}", "https://hackathon.com/test 123", "{user name}", "test 123")]

        [InlineData("{p1}", "", "{p1}", "")]
        [InlineData("{p1}", " ", "{p1}", " ")]
        [InlineData("{p1}", null, "{p1}", "")]
        [InlineData("{p1}", "1", "{p1}", "1")]
        [InlineData("{p1}", "www.something.org", "{p1}", "www.something.org")]
        [InlineData("www.something.org/{p1}/", "www.something.org/1/", "{p1}", "1")]
        [InlineData("www.something.org/{p1}/{p2}/", "www.something.org/1/2/", "{p1},{p2}", "1,2")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/", "www.something.org/1/2/3/", "{p1},{p2},{p3}", "1,2,3")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/", "www.something.org/1/2/3/4/", "{p1},{p2},{p3},{p4}", "1,2,3,4")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/", "www.something.org/1/2/3/4/5/", "{p1},{p2},{p3},{p4},{p5}", "1,2,3,4,5")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/", "www.something.org/1/2/3/4/5/6/", "{p1},{p2},{p3},{p4},{p5},{p6}", "1,2,3,4,5,6")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/", "www.something.org/1/2/3/4/5/6/7/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7}", "1,2,3,4,5,6,7")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/", "www.something.org/1/2/3/4/5/6/7/8/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8}", "1,2,3,4,5,6,7,8")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/", "www.something.org/1/2/3/4/5/6/7/8/9/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9}", "1,2,3,4,5,6,7,8,9")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10}", "1,2,3,4,5,6,7,8,9,10")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11}", "1,2,3,4,5,6,7,8,9,10,11")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12}", "1,2,3,4,5,6,7,8,9,10,11,12")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13}", "1,2,3,4,5,6,7,8,9,10,11,12,13")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/{p34}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/34/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33},{p34}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/{p34}/{p35}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/34/35/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33},{p34},{p35}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/{p34}/{p35}/{p36}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/34/35/36/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33},{p34},{p35},{p36}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/{p34}/{p35}/{p36}/{p37}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/34/35/36/37/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33},{p34},{p35},{p36},{p37}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/{p34}/{p35}/{p36}/{p37}/{p38}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/34/35/36/37/38/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33},{p34},{p35},{p36},{p37},{p38}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/{p34}/{p35}/{p36}/{p37}/{p38}/{p39}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/34/35/36/37/38/39/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33},{p34},{p35},{p36},{p37},{p38},{p39}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39")]
        [InlineData("www.something.org/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/{p34}/{p35}/{p36}/{p37}/{p38}/{p39}/{p40}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/34/35/36/37/38/39/40/", "{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33},{p34},{p35},{p36},{p37},{p38},{p39},{p40}", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40")]
        [InlineData("{p0}/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}/{p11}/{p12}/{p13}/{p14}/{p15}/{p16}/{p17}/{p18}/{p19}/{p20}/{p21}/{p22}/{p23}/{p24}/{p25}/{p26}/{p27}/{p28}/{p29}/{p30}/{p31}/{p32}/{p33}/{p34}/{p35}/{p36}/{p37}/{p38}/{p39}/{p40}/", "www.something.org/1/2/3/4/5/6/7/8/9/10/11/12/13/14/15/16/17/18/19/20/21/22/23/24/25/26/27/28/29/30/31/32/33/34/35/36/37/38/39/40/", "{p0},{p1},{p2},{p3},{p4},{p5},{p6},{p7},{p8},{p9},{p10},{p11},{p12},{p13},{p14},{p15},{p16},{p17},{p18},{p19},{p20},{p21},{p22},{p23},{p24},{p25},{p26},{p27},{p28},{p29},{p30},{p31},{p32},{p33},{p34},{p35},{p36},{p37},{p38},{p39},{p40}", "www.something.org,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40")]

        public void ValidateExtractVariablesMustSupportAllPatterns(string pattern, string url, string keys, string values, string splitter = ",")
        {
            var builder = FirstRound.Builder;
            var sut = builder.Parse(pattern);

            var expectedDic = new Dictionary<string, string>();
            var expectedKeys = keys.Split(splitter);
            var expectedValues = values.Split(splitter);

            for (int elementIndex = 0; elementIndex < expectedKeys.Length; elementIndex++)
            {
                if (string.IsNullOrEmpty(expectedKeys[elementIndex])) continue;
                expectedDic.Add(expectedKeys[elementIndex], expectedValues[elementIndex]);
            }

            var actual = new Dictionary<string, string>();
            var isMatch = sut.IsMatched(url);
            sut.ExtractVariables(url, actual);
            actual.Should().NotBeNull().And.BeEquivalentTo(expectedDic);
        }
    }
}
