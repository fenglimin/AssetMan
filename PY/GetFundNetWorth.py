from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
import time

s = Service(ChromeDriverManager().install())
driver = webdriver.Chrome(service=s)
driver.get('https://www.citicbank.com/personal/investment/lcjdxcpxx/proxy/')
selectFundType = Select(driver.find_element_by_class_name('cms_proxy_cplb'))

# select by visible text
selectFundType.select_by_visible_text('代销理财产品')

selectFundName = Select(driver.find_element_by_class_name('cms_proxy_vague_srh_type'))

# select by visible text
selectFundName.select_by_visible_text('产品代码')

driver.find_element_by_class_name('cms_proxy_vague_srh_input').send_keys('9N213010')
driver.find_element_by_class_name('cms_proxy_srh_btn').click()
time.sleep(3)

result = driver.find_elements_by_xpath("//td")

with open('readme.txt', 'w') as f:
    f.write(result[2].text)
    f.write(' ')
    f.write(result[9].text)
