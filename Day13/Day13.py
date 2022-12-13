from itertools import zip_longest
import functools

def is_in_right_order(left, right):
  
  if type(left) == int and type(right) == int:
    if left > right:
      return 'f'
    if left < right:
      return 't'
    return 'c'

  if type(left) == list or type(right) == list:
    if(type(left) != list):
      left = [left]

    if(type(right) != list):
      right = [right]
    
    pairs = zip_longest(left, right, fillvalue=None)
    for (leftItem, rightItem) in pairs:
      if(leftItem is None):
        return True
      if(rightItem is None):
        return False  
      
      result = is_in_right_order(leftItem, rightItem)
      if result == 'c':
        continue
      else:
        return True if result == 't' or result == True else False
    return 'c'

f = open("input.txt", "r", encoding='utf-8-sig')

input = f.read()

pairs = '\n'.join(input.split('\n\n')).split('\n')

pairs = [eval(pair) for pair in pairs]

pairs.append([[2]])
pairs.append([[6]])

sorted = sorted(pairs, key=functools.cmp_to_key(is_in_right_order))


print(sorted)