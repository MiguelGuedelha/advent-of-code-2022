from itertools import zip_longest
from functools import cmp_to_key, reduce

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
        return 1
      if(rightItem is None):
        return -1  
      
      result = is_in_right_order(leftItem, rightItem)
      if result == 'c':
        continue
      else:
        return 1 if result == 't' or result == True else -1
    return 'c'

f = open("input.txt", "r", encoding='utf-8-sig')

input = f.read()

pairs = '\n'.join(input.split('\n\n')).split('\n')

pairs = [eval(pair) for pair in pairs]

pairs.append([[2]])
pairs.append([[6]])

sorted = sorted(pairs, key=cmp_to_key(is_in_right_order))
sorted.reverse()

resultIndexes = [idx+1 for idx, item in enumerate(sorted) if item == [[6]] or item == [[2]]]
result = reduce(lambda a, b: a* b, resultIndexes)

print(result)