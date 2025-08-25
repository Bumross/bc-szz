def depth_level(text: str, brackets: tuple) -> int:
    """
    Vrací maximální úroveň zanoření závorek
    """

    if len(text) == 0:
        return 0
    
    if test_of_parantheses(text, brackets):
        depth = text.count(brackets[0])
        return depth
    else:
        raise ValueError ("Chybné uzávorkování")


def test_of_parantheses(text: str, brackets: tuple) -> bool:
    """
    Testuje, zda jsou kulaté závorky správně uzávorkované.
    Příklad chybného uzávorkování: ")(())("
    """
    para_count = 0

    if len(text) == 0:
        return False
    
    if len(brackets) != 2:
        raise ValueError (f"Chybí {"otevírací" if len(brackets[0]) == 2 else "uzavírací"} znak")
    
    for c in text:
        if c == brackets[0]:
            para_count += 1
        elif c == brackets[1]:
            para_count -= 1
        
        if para_count < 0:
            return False

    return para_count == 0
    

brackets = ("",")")
text = "(())"


print(test_of_parantheses(text, brackets))

print(depth_level(text, brackets))